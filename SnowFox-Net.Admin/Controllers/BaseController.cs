using Microsoft.AspNetCore.Mvc;
using SnowFox_Net.Shared.DTOs;
using SnowFox_Net.Shared.Entities.Admin;

namespace SnowFox_Net.Admin.Controllers
{
    /// <summary>
    /// 基础控制器
    /// </summary>
    [ApiController]
    [Route("api/Admin/[controller]/[action]")]
    public class BaseController: ControllerBase
    {
    }
}
