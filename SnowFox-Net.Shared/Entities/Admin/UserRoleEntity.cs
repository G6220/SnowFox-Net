namespace SnowFox_Net.Shared.Entities.Admin
{
    using FreeSql.DataAnnotations;
    using System;

    /// <summary>
    /// 用户角色关联表
    /// </summary>
    [Table(Name = "UserRole")]
    public class UserRoleEntity
    {
        /// <summary>
        /// Gets or sets the UserId
        /// 用户Id
        /// </summary>
        [Column(IsPrimary = true)]
        public uint UserId { get; set; }

        /// <summary>
        /// Gets or sets the RoleId
        /// 角色Id
        /// </summary>
        [Column(IsPrimary = true)]
        public uint RoleId { get; set; }

        /// <summary>
        /// Gets or sets the CreatedTime
        /// 角色分配时间
        /// </summary>
        [Column(IsNullable = false)]
        public DateTime AssignedTime { get; set; }
    }
}
