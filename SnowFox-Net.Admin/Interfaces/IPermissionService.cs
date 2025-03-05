using SnowFox_Net.Shared.DTOs;
using SnowFox_Net.Shared.Responses;

namespace SnowFox_Net.Admin.Interfaces
{
    /// <summary>
    /// 权限服务接口
    /// </summary>
    public interface IPermissionService
    {
        /// <summary>
        /// 创建权限
        /// </summary>
        /// <returns></returns>
        public Task CreatePermission(PermissionDto dto);

        /// <summary>
        /// 更新权限
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public Task UpdatePermission(PermissionDto dto);

        /// <summary>
        /// 删除权限
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public Task DeletePermission(uint Id);
    }
}
