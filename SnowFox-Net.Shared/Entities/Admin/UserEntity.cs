namespace SnowFox_Net.Shared.Entities.Admin
{
    using FreeSql.DataAnnotations;
    using SnowFox_Net.Shared.Enums;
    using System;

    /// <summary>
    /// 用户表
    /// </summary>
    [Table(Name = "User")]
    public class UserEntity
    {
        /// <summary>
        /// Gets or sets the Id
        /// 用户ID
        /// </summary>
        [Column(IsPrimary = true)]
        public uint Id { get; set; }

        /// <summary>
        /// Gets or sets the UserName
        /// 用户名
        /// </summary>
        [Column(IsNullable = false)]
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the PasswordHash
        /// 密码哈希
        /// </summary>
        [Column(IsNullable = false)]
        public string PasswordHash { get; set; }

        /// <summary>
        /// Gets or sets the Gender
        /// 性别
        /// </summary>
        [Column(IsNullable = false, MapType =typeof(byte))]
        public GenderEnum Gender { get; set; }

        /// <summary>
        /// Gets or sets the Birthday
        /// 生日
        /// </summary>
        [Column(IsNullable = true)]
        public DateOnly? Birthday { get; set; }

        /// <summary>
        /// Gets or sets the AvatarUrl
        /// 头像地址
        /// </summary>
        [Column(IsNullable = false)]
        public string AvatarUrl { get; set; }

        /// <summary>
        /// Gets or sets the Email
        /// 邮箱
        /// </summary>
        [Column(IsNullable = true)]
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the PhoneNumber
        /// 手机号
        /// </summary>
        [Column(IsNullable = true)]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Gets or sets the Status
        /// 用户状态
        /// </summary>
        [Column(IsNullable = false, MapType =typeof(byte))]
        public UserStatusEnum Status { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether EmailVerified
        /// 邮箱验证
        /// </summary>
        [Column(IsNullable = false)]
        public bool EmailVerified { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether PhoneVerified
        /// 手机验证
        /// </summary>
        [Column(IsNullable = false)]
        public bool PhoneVerified { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether TwoFactorEnabled
        /// 双因素验证
        /// </summary>
        [Column(IsNullable = false)]
        public bool TwoFactorEnabled { get; set; }
    }
}
