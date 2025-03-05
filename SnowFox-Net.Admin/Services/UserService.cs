namespace SnowFox_Net.Admin.Services
{
    using SnowFox_Net.Admin.Interfaces;
    using SnowFox_Net.Common.Encrypt;
    using SnowFox_Net.Common.Extensions;
    using SnowFox_Net.Common.Redis;
    using SnowFox_Net.Shared.DTOs;
    using SnowFox_Net.Shared.Entities.Admin;
    using SnowFox_Net.Shared.Enums;
    using SnowFox_Net.Shared.Exceptions;
    using SnowFox_Net.Shared.VOs;

    /// <summary>
    /// 用户服务
    /// </summary>
    public class UserService : IUserService
    {
        /// <summary>
        /// Defines the _logger
        /// </summary>
        private readonly ILogger _logger;

        /// <summary>
        /// Defines the _idSegment
        /// </summary>
        private readonly IdSegment _idSegment;

        /// <summary>
        /// Defines the _argon2Tool
        /// </summary>
        private readonly Argon2Tool _argon2Tool;

        public readonly JwtTool _jwtTool;

        /// <summary>
        /// Defines the _ib
        /// </summary>
        private readonly IdleBus<DBEnum, IFreeSql> _ib;

        /// <summary>
        /// Defines the _redisCluster
        /// </summary>
        private readonly RedisCluster _redisCluster;

        /// <summary>
        /// Defines the _redLock
        /// </summary>
        private readonly RedLock _redLock;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserService"/> class.
        /// </summary>
        /// <param name="logger">The logger<see cref="ILogger{UserService}"/></param>
        /// <param name="idSegment">The idSegment<see cref="IdSegment"/></param>
        /// <param name="argon2Tool">The argon2Tool<see cref="Argon2Tool"/></param>
        /// <param name="ib">The ib<see cref="IdleBus{DBEnum, IFreeSql}"/></param>
        /// <param name="redisCluster">The redisCluster<see cref="RedisCluster"/></param>
        /// <param name="redLock">The redLock<see cref="RedLock"/></param>
        public UserService(ILogger<UserService> logger, IdSegment idSegment, Argon2Tool argon2Tool,JwtTool jwtTool, IdleBus<DBEnum, IFreeSql> ib, RedisCluster redisCluster, RedLock redLock)
        {
            _logger = logger;
            _idSegment = idSegment;
            _argon2Tool = argon2Tool;
            _jwtTool = jwtTool;
            _ib = ib;
            _redisCluster = redisCluster;
            _redLock = redLock;
        }

        /// <summary>
        /// The CreateUser
        /// </summary>
        /// <param name="dto">The dto<see cref="UserDto"/></param>
        /// <returns>The <see cref="Task"/></returns>
        public async Task<string> CreateUser(UserDto dto)
        {
            if (dto == null || dto.UserName.IsNull() || dto.Password.IsNull())
            {
                throw new BizLogicException("参数错误");
            }
            var redLock = await _redLock.Lock($"Admin.SensitiveAction", TimeSpan.FromSeconds(3));

            if (redLock != null)
            {
                try
                {
                    var checkId = await _idSegment.Exists("Admin.User");
                    if(!checkId) await _idSegment.CreateSegment("Admin.User", 0, 100, "用户号段");
                    var entity = dto.GetUserEntity();
                    entity.Id = await _idSegment.GetNextUIntId("Admin.User");
                    entity.PasswordHash = _argon2Tool.HashPassword(dto.Password);
                    var freesql = _ib.Get(DBEnum.Default);
                    var roles = await freesql.Select<RoleEntity>().Where(a => a.Status == StatusEnum.默认).ToListAsync<Role>();
                    var roleIds = roles.Select(a => a.Id).ToList();
                    var roleInherits = await freesql.Select<RoleHierarchyEntity>().Where(a => roleIds.Contains(a.ParrentRoleId)).ToListAsync();
                    var roleInheritIds = roleInherits.Select(a => a.RoleId).ToList();
                    var userRoles = new List<UserRoleEntity>();
                    roleIds.ForEach(roleId =>
                    {
                        var userRole = new UserRoleEntity();
                        userRole.RoleId = roleId;
                        userRole.UserId = entity.Id;
                        userRoles.Add(userRole);
                    });

                    var userInheritRoles = new List<UserRoleInheritanceEntity>();
                    roleInheritIds.ForEach(roleInheritId =>
                    {
                        var userInheritRole = new UserRoleInheritanceEntity();
                        userInheritRole.RoleId = roleInheritId;
                        userInheritRole.UserId = entity.Id;
                        userInheritRoles.Add(userInheritRole);
                    });

                    freesql.Transaction(() =>
                    {
                        freesql.Insert<UserEntity>().AppendData(entity).ExecuteAffrows();
                        freesql.Insert<UserRoleEntity>().AppendData(userRoles).ExecuteAffrows();
                        freesql.Insert<UserRoleInheritanceEntity>().AppendData(userInheritRoles).ExecuteAffrows();

                    });


                    var permissions = await freesql.Select<RolePermissionEntity, PermissionEntity>().InnerJoin(s => s.t1.PermissionId == s.t2.Id).ToListAsync<Permission>(s => new Permission
                    {
                        Id = s.t2.Id,
                        Type = s.t2.Type,
                        Value = s.t2.Value
                    });
                    var claims = new Token();
                    claims.Id = entity.Id;
                    claims.UserName = entity.UserName;
                    claims.Status = entity.Status;
                    claims.EmailVerified = entity.EmailVerified;
                    claims.PhoneVerified = entity.PhoneVerified;
                    claims.TwoFactorEnabled = entity.TwoFactorEnabled;
                    claims.Roles = roles;
                    claims.Permissions = permissions;
                    var token =  _jwtTool.GenerateToken(claims);
                    return token;

                }
                catch (Exception ex)
                {
                    if (ex.GetType() != typeof(BizLogicException))
                    {
                        _logger.LogError(ex, "注册失败");
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
                _logger.LogWarning($"抢锁失败");
                throw new BizLogicException("服务器繁忙,请稍后再试");
            }
        }
    }
}
