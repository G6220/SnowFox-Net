namespace SnowFox_Net.Common.Redis
{
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;
    using SnowFox_Net.Common.Extensions;
    using SnowFox_Net.Shared.DTOs;
    using SnowFox_Net.Shared.Entities.Common;
    using SnowFox_Net.Shared.Enums;
    using SnowFox_Net.Shared.Exceptions;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="IdSegment" />
    /// </summary>
    public class IdSegment
    {

        /// <summary>
        /// Defines the _logger
        /// </summary>
        private readonly ILogger _logger;

        /// <summary>
        /// Defines the _redLock
        /// </summary>
        private readonly RedLock _redLock;

        /// <summary>
        /// Defines the _ib
        /// </summary>
        private readonly IdleBus<DBEnum, IFreeSql> _ib;

        /// <summary>
        /// Defines the _redisCache
        /// </summary>
        private readonly RedisCache _redisCache;

        public IdSegment(ILogger<IdSegment> logger, RedLock redLock, IdleBus<DBEnum, IFreeSql> ib, RedisCache redisCache)
        {
            _logger = logger;
            _redLock = redLock;
            _ib = ib;
            _redisCache = redisCache;
        }

        /// <summary>
        /// 获取下一个 ID
        /// </summary>
        /// <param name="bizName">The bizName<see cref="string"/></param>
        /// <returns>The <see cref="Task{uint}"/></returns>
        public async Task<uint> GetNextUIntId(string bizName)
        {
            // 先从 Redis 获取当前编号
            var redisKey = $"segment:{bizName}";
            IdSegmentDto segment = null;
            var json = _redisCache.Get(redisKey);
            if (json.NotNull())
            {
                segment = JsonConvert.DeserializeObject<IdSegmentDto>(json);
            }
            if (segment == null || segment.CurrentId >= segment.MaxId)
            {
                segment = await GetNextSegmentAsync(bizName);
            }
            segment.CurrentId++;
            // 生成下一个 ID
            var nextId = segment.CurrentId;

            // 更新 Redis 
            var success = _redisCache.Set(redisKey, JsonConvert.SerializeObject(segment));
            if (!success)
            {
                throw new BizLogicException($"更新号段失败，Key: {redisKey}");
            }
            return (uint)nextId;
        }

        /// <summary>
        /// 获取下一个 ID
        /// </summary>
        /// <param name="bizName">The bizName<see cref="string"/></param>
        /// <returns>The <see cref="Task{ulong}"/></returns>
        public async Task<ulong> GetNextULongId(string bizName)
        {
            // 先从 Redis 获取当前编号
            var redisKey = $"segment:{bizName}";
            IdSegmentDto segment = null;
            var json = _redisCache.Get(redisKey);
            if (json.NotNull())
            {
                segment = JsonConvert.DeserializeObject<IdSegmentDto>(json);
            }
            if (segment == null || segment.CurrentId >= segment.MaxId)
            {
                segment = await GetNextSegmentAsync(bizName);
            }
            segment.CurrentId++;
            // 生成下一个 ID
            var nextId = segment.CurrentId;

            // 更新 Redis 
            var success = _redisCache.Set(redisKey, JsonConvert.SerializeObject(segment));
            if (!success)
            {
                throw new BizLogicException($"更新号段失败，Key: {redisKey}");
            }
            return nextId;
        }

        /// <summary>
        /// 从数据库获取下一号段
        /// </summary>
        /// <param name="bizName">The bizName<see cref="string"/></param>
        /// <returns>The <see cref="Task{IdSegmentDto}"/></returns>
        private async Task<IdSegmentDto> GetNextSegmentAsync(string bizName)
        {
            var redLock = await _redLock.Lock($"IdSegment:{bizName}", TimeSpan.FromSeconds(3));
            if (redLock != null)
            {
                try
                {
                    var freesql = _ib.Get(DBEnum.Common);
                    var exists = await Exists(bizName);
                    if (!exists)
                    {
                        throw new InvalidOperationException($"未找到业务名为 {bizName} 的 ID 段信息");
                    }
                    IdSegmentDto segment = null;
                    freesql.Transaction(() =>
                    {
                        segment = freesql.Select<IdSegmentEntity>()
                           .Where(a => a.BizName == bizName)
                           .ForUpdate()
                           .ToOne<IdSegmentDto>();
                        segment.CurrentId = segment.MaxId;
                        segment.MaxId += segment.Step;
                        segment.Version++;
                        freesql.Update<IdSegmentEntity>()
                            .Set(a => a.MaxId, segment.MaxId)
                            .Set(a => a.Version, segment.Version)
                            .Where(a => a.BizName == bizName)
                            .ExecuteAffrows();
                    });
                    return segment;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"获取号段失败，BizName: {bizName}");
                    throw;
                }
                finally
                {
                    await _redLock.Unlock(redLock);
                }
            }
            else
            {
                _logger.LogWarning($"获取号段失败，BizName: {bizName}");
                throw new BizLogicException($"服务器繁忙,请稍后再试");
            }
        }

        /// <summary>
        /// 检查号段是否存在
        /// </summary>
        /// <param name="bizName"></param>
        /// <returns></returns>
        public async Task<bool> Exists(string bizName)
        {
            var freesql = _ib.Get(DBEnum.Common);
            return await freesql.Select<IdSegmentEntity>()
                          .Where(a => a.BizName == bizName)
                          .CountAsync() > 0;
        }

        /// <summary>
        /// The CreateSegment
        /// </summary>
        /// <param name="bizName">The bizName<see cref="string"/></param>
        /// <param name="maxId">The maxId<see cref="ulong"/></param>
        /// <param name="step">The step<see cref="ushort"/></param>
        /// <param name="description">The description<see cref="string"/></param>
        /// <returns>The <see cref="Task"/></returns>
        public async Task CreateSegment(string bizName, ulong maxId, ushort step, string description)
        {
            var redLock = await _redLock.Lock($"IdSegment:{bizName}", TimeSpan.FromSeconds(3));
            if (redLock != null)
            {
                try
                {
                    var freesql = _ib.Get(DBEnum.Common);
                    var entity = new IdSegmentEntity
                    {
                        BizName = bizName,
                        MaxId = maxId,
                        Step = step,
                        Version = 0,
                        Description = description
                    };

                    var sql= freesql.Insert<IdSegmentEntity>(entity).ToSql();
                    await freesql.Insert<IdSegmentEntity>(entity).ExecuteAffrowsAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"创建号段失败，BizName: {bizName}");
                    throw;
                }
                finally
                {
                    await _redLock.Unlock(redLock);
                }
            }
            else
            {
                _logger.LogWarning($"创建号段失败，BizName: {bizName}");
                throw new BizLogicException($"服务器繁忙,请稍后再试");
            }
        }
    }
}
