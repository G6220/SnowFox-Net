using FreeSql.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnowFox_Net.Shared.Entities.Admin
{
    /// <summary>
    /// 角色互斥关系表
    /// </summary>
    [Table(Name = "RoleMutualExclusion")]
    public class RoleMutualExclusionEntity
    {
        /// <summary>
        /// 角色ID_A
        /// </summary>
        [Column(IsPrimary = true)]
        public uint RoleIdA { get; set; }
        /// <summary>
        /// 角色ID_B
        /// </summary>
        [Column(IsPrimary = true)]
        public uint RoleIdB { get; set; }

        /// <summary>
        /// Gets or sets the CreatedTime
        /// 创建时间
        /// </summary>
        [Column(IsNullable = false)]
        public DateTime CreatedTime { get; set; }
    }
}
