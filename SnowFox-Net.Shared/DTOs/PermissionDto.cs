using SnowFox_Net.Shared.Entities.Admin;
using SnowFox_Net.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnowFox_Net.Shared.DTOs
{
   public class PermissionDto
    {
        /// <summary>
        /// Gets or sets the Id
        /// 权限ID
        /// </summary>
        public uint? Id { get; set; }

        /// <summary>
        /// Gets or sets the Id
        /// 父ID——树形结构使用
        /// </summary>
        public uint? PId { get; set; }

        /// <summary>
        /// Gets or sets the Type
        /// 权限类型
        /// </summary>
        public PermissionTypeEnum Type { get; set; }

        /// <summary>
        /// Gets or sets the Name
        /// 权限名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the Value
        /// 权限值
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// 权限描述
        /// </summary>
        public string Description { get; set; }

        ///
        public PermissionEntity GetPermissionEntity()
        {
            return new PermissionEntity
            {
                PId = PId,
                Type = Type,
                Name = Name,
                Value = Value,
                Description = Description
            };
        }
    }
}
