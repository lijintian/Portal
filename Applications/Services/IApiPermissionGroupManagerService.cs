using System.Collections.Generic;
using Portal.Dto;

namespace Portal.Applications.Services
{
    /// <summary>
    /// 表示API权限分组管理服务
    /// </summary>
    public interface IApiPermissionGroupManagerService
    {
        /// <summary>
        /// 保存API权限分组
        /// </summary>
        /// <param name="role">API权限分组信息</param>
        /// <returns>保存的API权限分组</returns>
        ApiPermissionGroup Save(ApiPermissionGroup role, SysLoggerDto logger);

        /// <summary>
        /// 获据id获取ApiPermissionGroup信息
        /// </summary>
        ApiPermissionGroup GetById(string id);

        /// <summary>
        /// 获取code获取ApiPermissionGroup信息
        /// </summary>
        ApiPermissionGroup GetByCode(string code);
        /// <summary>
        /// 检查code是否唯一
        /// </summary>
        bool IsUniqueCode(string code);


        IEnumerable<ApiPermissionGroup> GetList(FindApiPermissionGroupRequest request);

        /// <summary>
        /// 获取API权限分组的权限列表
        /// </summary>
        /// <returns></returns>
        IEnumerable<Permission> GetApiPermissionGroupPermissions(string code);

        IEnumerable<ApiPermissionGroup> GetList();

        IEnumerable<ApiPermissionGroup> GetList(string[] code);

        /// <summary>
        /// 获取对外开放的API权限分组
        /// </summary>
        /// <returns></returns>
        List<ApiPermissionGroup> GetOpenedGroupList(DeveloperAppDto info);
    }
}
