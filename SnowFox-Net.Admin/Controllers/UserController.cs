using Microsoft.AspNetCore.Mvc;
using SnowFox_Net.Admin.Interfaces;
using SnowFox_Net.Common.Extensions;
using SnowFox_Net.Shared.DTOs;
using SnowFox_Net.Shared.Entities.Admin;
using SnowFox_Net.Shared.Responses;

namespace SnowFox_Net.Admin.Controllers
{
    /// <summary>
    /// 用户控制器
    /// </summary>
    public class UserController : BaseController
    {
        private readonly ILogger _logger;
        private readonly IUserService _userService;

        public UserController(ILogger<UserController> logger, IUserService userService)
        {
            _logger = logger;
            _userService = userService;
        }

        [HttpPost]
        public async Task<IResponseOutput> Register(UserDto dto)
        {
            var data = await _userService.CreateUser(dto);
            return ResponseOutput.OK(data);
        }
    }
}
