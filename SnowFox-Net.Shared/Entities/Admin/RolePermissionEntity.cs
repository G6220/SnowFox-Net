namespace SnowFox_Net.Shared.Entities.Admin
{
    using FreeSql.DataAnnotations;
    using System;

    /// <summary>
    /// 角色权限关联表
    /// </summary>
    [Table(Name = "RolePermission")]
    public class RolePermissionEntity
    {
        /// <summary>
        /// Gets or sets the RoleId
        /// 角色Id
        /// </summary>
        [Column(IsPrimary = true)]
        public uint RoleId { get; set; }

        /// <summary>
        /// Gets or sets the PermissionId
        /// 权限Id
        /// </summary>
        [Column(IsPrimary = true)]
        public uint PermissionId { get; set; }

        /// <summary>
        /// Gets or sets the CreatedTime
        /// 权限授予时间
        /// </summary>
        [Column(IsNullable = false)]
        public DateTime GrantedTime { get; set; }
    }
}
