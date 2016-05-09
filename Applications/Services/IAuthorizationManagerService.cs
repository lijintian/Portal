using System.Collections.Generic;
using System.Data;
using Portal.Applications.Events;
using Portal.Domain.Aggregates.UserAgg.Events;
using Portal.Dto;
using Portal.Dto.Request;
using Portal.Web.Core.Model;

namespace Portal.Applications.Services
{
    /// <summary>
    /// 表示授权管理服务
    /// </summary>
    public interface IAuthorizationManagerService
    {
        /// <summary>
        /// 保存用户的角色列表
        /// </summary>
        void SaveUserToRoles(string userId, IEnumerable<string> roleCodes, SysLoggerDto logger);
        /// <summary>
        /// 保存角色的权限列表
        /// </summary>
        void SavePermissionsToRole(string roleId, IEnumerable<string> permissionCodes, SysLoggerDto logger);
        /// <summary>
        /// 保存API权限分组的权限列表
        /// </summary>
        void SavePermissionsToApiGroup(string code, IEnumerable<string> permissionCodes, SysLoggerDto logger);
        /// <summary>
        /// 保存用户的权限列表
        /// </summary>
        void SaveUserOwnedPermissions(string userId, IEnumerable<string> permissionCodes, SysLoggerDto logger);
    }
}
