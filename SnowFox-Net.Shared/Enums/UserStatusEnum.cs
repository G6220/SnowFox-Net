using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnowFox_Net.Shared.Enums
{
    public enum UserStatusEnum:byte
    {
        正常 = 0,
        禁用 = 1,
        注销 = 7,
        删除 = 9
    }
}
