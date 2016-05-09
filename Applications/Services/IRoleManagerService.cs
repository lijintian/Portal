using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Portal.Dto;

namespace Portal.Applications.Services
{
    /// <summary>
    /// 表示角色管理服务
    /// </summary>
    public interface IRoleManagerService
    {
        /// <summary>
        /// 保存角色
        /// </summary>
        /// <param name="role">角色信息</param>
        /// <returns>保存的角色</returns>
        Role Save(Role role, SysLoggerDto logger);

        /// <summary>
        /// 获据id获取Role信息
        /// </summary>
        Role GetById(string id);

        /// <summary>
        /// 获取code获取Role信息
        /// </summary>
        Role GetByCode(string code);
        /// <summary>
        /// 检查code是否唯一
        /// </summary>
        bool IsUniqueCode(string code);

        /// <summary>
        /// 获取角色的权限列表
        /// </summary>
        /// <returns></returns>
        IEnumerable<Permission> GetRolePermissions(string code); 
       
        /// <summary>
        /// 获取指定应用的角色列表
        /// </summary>
        IEnumerable<Role> GetByApplicationId(string appId);
    }
}
