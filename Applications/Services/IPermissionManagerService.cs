using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;

using Portal.Dto;

namespace Portal.Applications.Services
{
    /// <summary>
    /// 表示权限管理服务
    /// </summary>
    public interface IPermissionManagerService
    {
        /// <summary>
        /// 保存页面权限
        /// </summary>
        PagePermission Save(PagePermission pagePer, SysLoggerDto logger);

        /// <summary>
        /// 保存Api权限
        /// </summary>
        ApiPermission Save(ApiPermission apiPer, SysLoggerDto logger);

        /// <summary>
        /// 权限码是否唯用
        /// </summary>
        bool IsUniqueCode(string perCode);

        /// <summary>
        /// 删除某一权限，将不会实现
        /// </summary>
        void Remove(string id);

        /// <summary>
        /// 根据code查找特定权限, 仅支持页面权限和API权限
        /// </summary>
        T GetByCode<T>(string code) where T : Permission;

        /// <summary>
        ///  根据查询条件查找特定权限, 仅支持页面权限和API权限
        /// </summary>
        FindPermissionResponse<T> FindPermissions<T>(FindPermissionRequest reqeust) where T : ApplicationPermission;

        IEnumerable<T> GetList<T>(FindPermissionRequest reqeust) where T : ApplicationPermission;

        /// <summary>
        /// 获取所有页面权限列表
        /// </summary>
        /// <returns></returns>
        IEnumerable<PagePermission> GetPagePermissionList(string applicationId);

        IEnumerable<AppPermission> GetApiPermissionList(string[] code);
    }
}
