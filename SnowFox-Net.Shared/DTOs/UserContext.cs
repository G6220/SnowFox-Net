using SnowFox_Net.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnowFox_Net.Shared.DTOs
{
    /// <summary>
    /// 用户上下文信息
    /// </summary>
    public class UserContext
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public uint Id { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 用户状态
        /// </summary>

        public UserStatusEnum Status { get; set; }

        /// <summary>
        /// 邮箱验证
        /// </summary>

        public bool EmailVerified { get; set; }

        /// <summary>
        /// 手机验证
        /// </summary>
        public bool PhoneVerified { get; set; }

        /// <summary>
        /// 双重验证
        /// </summary>
        public bool TwoFactorEnabled { get; set; }

        /// <summary>
        /// 角色
        /// </summary>
        public List<Role> Roles { get; set; }

        /// <summary>
        /// 权限
        /// </summary>
        public List<Permission> Permissions { get; set; }
    }

    public class Role
    {
        /// <summary>
        /// 角色Id
        /// </summary>
        public uint Id { get; set; }

        /// <summary>
        /// 角色名
        /// </summary>
        public string Name { get; set; }
    }

    public class Permission
    {
        /// <summary>
        /// 权限Id
        /// </summary>
        public uint Id { get; set; }

        /// <summary>
        /// 权限类型
        /// </summary>
        public PermissionTypeEnum Type { get; set; }

        /// <summary>
        /// 权限值
        /// </summary>
        public string Value { get; set; }
    }
}
