using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnowFox_Net.Shared.DTOs
{
    /// <summary>
    /// 号段DTO
    /// </summary>
    public class IdSegmentDto
    {
        /// <summary>
        /// 业务名
        /// </summary>
        public string BizName { get; set; }

        /// <summary>
        /// 最大ID
        /// </summary>
        public ulong MaxId { get; set; }

        /// <summary>
        /// 当前ID
        /// </summary>
        public ulong CurrentId { get; set; }

        /// <summary>
        /// 步长
        /// </summary>
        public ushort Step { get; set; }

        /// <summary>
        /// 版本号
        /// </summary>
        public uint Version { get; set; }

        /// <summary>
        /// 业务描述
        /// </summary>
        public string Description { get; set; }
    }
}
