using FreeSql.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnowFox_Net.Shared.Entities.Common
{
    /// <summary>
    /// Id号段表
    /// </summary>
    [Table(Name = "IdSegment")]
    public class IdSegmentEntity
    {
        /// <summary>
        /// 业务名
        /// </summary>
        [Column(IsPrimary = true)]
        public string BizName { get; set; }

        /// <summary>
        /// 最大ID
        /// </summary>
        [Column(IsNullable = false)]
        public ulong MaxId { get; set; }

        /// <summary>
        /// 步长
        /// </summary>
        [Column(IsNullable = false)]
        public ushort Step { get; set; }

        /// <summary>
        /// 版本号
        /// </summary>
        [Column(IsNullable = false)]
        public uint Version { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [Column(IsNullable = false)]
        public DateTime CreatedTime { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        [Column(IsNullable = false)]
        public DateTime UpdatedTime { get; set; }

        /// <summary>
        /// 业务描述
        /// </summary>
        [Column(IsNullable = false)]
        public string Description { get; set; }
    }
}
