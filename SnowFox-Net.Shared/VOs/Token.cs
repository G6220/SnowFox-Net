using SnowFox_Net.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnowFox_Net.Shared.VOs
{
    public class Token:UserContext
    {
        /// <summary>
        /// 生效时间
        /// </summary>
        public DateTime IssuedTime {  get; set; }

        /// <summary>
        /// 过期时间
        /// </summary>
        public DateTime ExpTime {  get; set; }

    }
}
