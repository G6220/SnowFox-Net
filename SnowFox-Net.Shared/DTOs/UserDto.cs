using SnowFox_Net.Shared.Entities.Admin;
using SnowFox_Net.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnowFox_Net.Shared.DTOs
{
    /// <summary>
    /// 用户DTO
    /// </summary>
    public class UserDto
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public uint Id { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public GenderEnum? Gender { get; set; }

        /// <summary>
        /// 生日
        /// </summary>
        public DateOnly? Birthday { get; set; }

        /// <summary>
        /// Gets or sets the AvatarUrl
        /// 头像地址
        /// </summary>
        public string AvatarUrl { get; set; }

        /// <summary>
        /// Gets or sets the Email
        /// 邮箱
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the PhoneNumber
        /// 手机号
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Gets or sets the Status
        /// 用户状态
        /// </summary>
        public UserStatusEnum? Status { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether EmailVerified
        /// 邮箱验证
        /// </summary>
        public bool? EmailVerified { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether PhoneVerified
        /// 手机验证
        /// </summary>
        public bool? PhoneVerified { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether TwoFactorEnabled
        /// 双因素验证
        /// </summary>
        public bool? TwoFactorEnabled { get; set; }

        public UserEntity GetUserEntity()
        {
            return new UserEntity
            {
                UserName = this.UserName,
                Gender = this.Gender ?? GenderEnum.未知,
                Birthday = this.Birthday,
                AvatarUrl = this.AvatarUrl,
                Email = this.Email,
                PhoneNumber = this.PhoneNumber,
                Status = this.Status??UserStatusEnum.正常,
                EmailVerified = this.EmailVerified??false,
                PhoneVerified = this.PhoneVerified??false,
                TwoFactorEnabled = this.TwoFactorEnabled??false
            };
        }

    }
}
