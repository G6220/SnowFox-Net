namespace SnowFox_Net.Shared.Entities.Admin
{
    using FreeSql.DataAnnotations;
    using SnowFox_Net.Shared.Enums;

    /// <summary>
    /// 权限表
    /// </summary>
    [Table(Name = "Permission")]
    public class PermissionEntity
    {
        /// <summary>
        /// Gets or sets the Id
        /// 权限ID
        /// </summary>
        [Column(IsPrimary = true)]
        public uint Id { get; set; }

        /// <summary>
        /// Gets or sets the Id
        /// 父ID——树形结构使用
        /// </summary>
        public uint? PId { get; set; }

        /// <summary>
        /// Gets or sets the Type
        /// 权限类型
        /// </summary>
        [Column(IsNullable = false)]
        public PermissionTypeEnum Type { get; set; }

        /// <summary>
        /// Gets or sets the Name
        /// 权限名
        /// </summary>
        [Column(IsNullable = false)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the Value
        /// 权限值
        /// </summary>
        [Column(IsNullable = false)]
        public string Value { get; set; }

        /// <summary>
        /// 权限描述
        /// </summary>
        [Column(IsNullable = false)]
        public string Description {  get; set; }

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
        public string CreatedUserName { get; set; }

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
        public string UpdatedUserName { get; set; }
    }
}
