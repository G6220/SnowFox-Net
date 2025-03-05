using FreeSql.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnowFox_Net.Shared.Entities.Admin
{
    /// <summary>
    /// 用户角色继承关系表
    /// </summary>
    [Table(Name = "UserRoleInheritance")]
    public class UserRoleInheritanceEntity
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        [Column(IsPrimary = true)]
        public uint UserId {  get; set; }

        /// <summary>
        /// 角色ID
        /// </summary>
        [Column(IsPrimary = true)]
        public uint RoleId { get; set; }

        /// <summary>
        /// 继承时间
        /// </summary>
        [Column(IsNullable = false)]
        public DateTime InheritedTime {  get; set; }
    }
}
