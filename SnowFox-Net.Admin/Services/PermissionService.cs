using SnowFox_Net.Admin.Interfaces;
using SnowFox_Net.Common.Extensions;
using SnowFox_Net.Common.Redis;
using SnowFox_Net.Shared.DTOs;
using SnowFox_Net.Shared.Entities.Admin;
using SnowFox_Net.Shared.Enums;
using SnowFox_Net.Shared.Exceptions;

namespace SnowFox_Net.Admin.Services
{
    /// <summary>
    /// 权限服务
    /// </summary>
    public class PermissionService : IPermissionService
    {

        private readonly ILogger _logger;
        private readonly IdSegment _idSegment;
        private readonly IdleBus<DBEnum,IFreeSql> _ib;
        private readonly RedLock _redLock;
        private readonly UserContext _user;

        public PermissionService(ILogger<PermissionService> logger, IdSegment idSegment, IdleBus<DBEnum, IFreeSql> ib, RedLock redLock, UserContext user)
        {
            _logger = logger;
            _idSegment = idSegment;
            _ib = ib;
            _redLock = redLock;
            _user = user;
        }

        /// <summary>
        /// 创建权限
        /// </summary>
        /// <returns></returns>
        public async Task CreatePermission(PermissionDto dto)
        {
            if(dto.Name.IsNull()||dto.Value.IsNull()||dto.Description.IsNull())
            {
                throw new BizLogicException("参数错误");
            }
            var redLock = await _redLock.Lock($"Admin.SensitiveAction", TimeSpan.FromSeconds(3));

            if(redLock != null)
            {
                try
                {
                    var freesql = _ib.Get(DBEnum.Default);
                    var entity = dto.GetPermissionEntity();
                    var checkId = await _idSegment.Exists("Admin.Permission");
                    if(!checkId) await _idSegment.CreateSegment("Admin.Permission", 0, 100, "权限号段");
                    entity.Id = await _idSegment.GetNextUIntId("Admin.Permission");
                    entity.CreatedTime = DateTime.Now;
                    entity.CreatedUserId = _user.Id;
                    entity.CreatedUserName = _user.UserName;
                    entity.UpdatedTime = entity.CreatedTime;
                    entity.UpdatedUserId = entity.CreatedUserId;
                    entity.UpdatedUserName = entity.CreatedUserName;
                    freesql.Transaction(() =>
                    {
                        var check =  freesql.Select<PermissionEntity>()
                        .Where(a => a.Value == dto.Value)
                        .Count();
                        if (check > 0) throw new BizLogicException("创建权限失败,权限已存在");
                        freesql.Insert(entity).ExecuteAffrows();
                    });

                }
                catch (Exception ex)
                {
                    if(ex.GetType() != typeof(BizLogicException))
                    {
                        _logger.LogError(ex, "创建权限失败");
                    }
                    throw;
                }
                finally
                {
                    await _redLock.Unlock(redLock);
                }
            }
            else
            {
                _logger.LogWarning("抢锁失败");
                throw new BizLogicException("服务器繁忙,请稍后再试");
            }

        }

        /// <summary>
        /// 更新权限
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task UpdatePermission(PermissionDto dto)
        {
            if (dto.Id==null||dto.Name.IsNull() || dto.Value.IsNull() || dto.Description.IsNull())
            {
                throw new BizLogicException("参数错误");
            }
            var redLock = await _redLock.Lock($"Admin.SensitiveAction", TimeSpan.FromSeconds(3));

            if (redLock != null)
            {
                try
                {
                    var freesql = _ib.Get(DBEnum.Default);

                    var entity = dto.GetPermissionEntity();
                    entity.UpdatedTime = DateTime.Now;
                    entity.UpdatedUserId = _user.Id;
                    entity.UpdatedUserName = _user.UserName;
                    freesql.Transaction(() =>
                    {
                        var check = freesql.Select<PermissionEntity>()
                        .Where(a => a.Value == dto.Value&&a.Id!=dto.Id)
                        .Count();
                        if (check > 0) throw new BizLogicException("更新权限失败,权限已存在");
                        freesql.Update<PermissionEntity>(entity).ExecuteAffrows();
                    });

                }
                catch (Exception ex)
                {
                    if (ex.GetType() != typeof(BizLogicException))
                    {
                        _logger.LogError(ex, "更新权限失败");
                    }
                    throw;
                }
                finally
                {
                    await _redLock.Unlock(redLock);
                }
            }
            else
            {
                _logger.LogWarning("抢锁失败");
                throw new BizLogicException("服务器繁忙,请稍后再试");
            }
        }

        /// <summary>
        /// 删除权限
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task DeletePermission(uint Id)
        {
            var redLock = await _redLock.Lock($"Admin.SensitiveAction", TimeSpan.FromSeconds(3));

            if (redLock != null)
            {
                try
                {
                    var freesql = _ib.Get(DBEnum.Default);
                    freesql.Transaction(() =>
                    {
                        var childs = freesql.Select<PermissionEntity>().Where(a => a.PId == Id).Count();
                        if (childs > 0) throw new BizLogicException("删除权限失败,存在子权限未删除");
                        freesql.Delete<PermissionEntity>().Where(a=>a.Id==Id).ExecuteAffrows();
                        freesql.Delete<RolePermissionEntity>().Where(a => a.PermissionId == Id).ExecuteAffrows();
                    });

                }
                catch (Exception ex)
                {
                    if (ex.GetType() != typeof(BizLogicException))
                    {
                        _logger.LogError(ex, "删除权限失败");
                    }
                    throw;
                }
                finally
                {
                    await _redLock.Unlock(redLock);
                }
            }
            else
            {
                _logger.LogWarning("抢锁失败");
                throw new BizLogicException("服务器繁忙,请稍后再试");
            }
        }
    }
}
