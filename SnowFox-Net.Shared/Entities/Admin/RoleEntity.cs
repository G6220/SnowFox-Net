namespace SnowFox_Net.Shared.Entities.Admin
{
    using FreeSql.DataAnnotations;
    using SnowFox_Net.Shared.Enums;

    /// <summary>
    /// 角色表
    /// </summary>
    [Table(Name = "Role")]
    public class RoleEntity
    {
        /// <summary>
        /// Gets or sets the Id
        /// 角色ID
        /// </summary>
        [Column(IsPrimary = true)]
        public uint Id { get; set; }

        /// <summary>
        /// Gets or sets the Name
        /// 角色名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the Status
        /// 角色状态
        /// </summary>
        public StatusEnum Status { get; set; }

        /// <summary>
        /// Gets or sets the Description
        /// 角色描述
        /// </summary>
        public string Description { get; set; }


        /// <summary>
        /// Gets or sets the CreatedTime
        /// 创建时间
        /// </summary>
        [Column(IsNullable = false)]
        public DateTime CreatedTime { get; set; }

        /// <summary>
        /// Gets or sets the CreatedUserId
        /// 创建人Id
        /// </summary>
        [Column(IsNullable = false)]
        public uint CreatedUserId { get; set; }

        /// <summary>
        /// Gets or sets the CreateUserName
        /// 创建人
        /// </summary>
        [Column(IsNullable = false)]
        public string CreateUserName { get; set; }

        /// <summary>
        /// Gets or sets the UpdatedTime
        /// 更新时间
        /// </summary>
        [Column(IsNullable = false)]
        public DateTime UpdatedTime { get; set; }

        /// <summary>
        /// Gets or sets the UpdatedUserId
        /// 更新人Id
        /// </summary>
        [Column(IsNullable = false)]
        public uint UpdatedUserId { get; set; }

        /// <summary>
        /// Gets or sets the UpdateUserName
        /// 更新人
        /// </summary>
        [Column(IsNullable = false)]
        public string UpdateUserName { get; set; }
    }
}
