using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnowFox_Net.Shared.Requests.Admin
{
    /// <summary>
    /// 用户请求接口
    /// </summary>
    public class UserRequest: BaseRequest
    {
        public string UserName { set; get;}
        public string PassWord { set;get; }
    }
}
