using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnowFox_Net.Shared.Enums
{
    
    public enum PermissionTypeEnum:byte
    {
        /// <summary>
        /// 模块权限
        /// </summary>
        Module=0,

        /// <summary>
        /// 视图权限
        /// </summary>
        View = 1,

        /// <summary>
        /// 操作权限
        /// </summary>
        Action = 2,

        /// <summary>
        /// 字段权限
        /// </summary>
        Column = 3  

    }
}
