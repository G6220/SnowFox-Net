namespace SnowFox_Net.Shared.Entities.Admin
{
    using FreeSql.DataAnnotations;
    using System;

    /// <summary>
    /// 角色层级关系实体
    /// </summary>
    [Table(Name = "RoleHierarchy")]
    public class RoleHierarchyEntity
    {
        /// <summary>
        /// Gets or sets the RoleId
        /// 角色Id
        /// </summary>
        [Column(IsPrimary = true)]
        public uint RoleId { get; set; }

        /// <summary>
        /// Gets or sets the ParrentRoleId
        /// 父角色Id
        /// </summary>
        [Column(IsPrimary = true)]
        public uint ParrentRoleId { get; set; }

        /// <summary>
        /// Gets or sets the CreatedTime
        /// 继承时间
        /// </summary>
        [Column(IsNullable = false)]
        public DateTime CreatedTime { get; set; }
    }
}
