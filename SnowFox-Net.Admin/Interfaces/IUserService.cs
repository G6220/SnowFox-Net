using SnowFox_Net.Shared.DTOs;

namespace SnowFox_Net.Admin.Interfaces
{
    /// <summary>
    /// 用户服务接口
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// 创建用户
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public Task<string> CreateUser(UserDto dto);
    }
}
