using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using System.Security.Cryptography.X509Certificates;
using Portal.Dto;
using Portal.Dto.Request;
using Portal.Dto.Response;

namespace Portal.Applications.Services
{
    /// <summary>
    /// 表示用户管理服务
    /// </summary>
    public interface IUserManagerService
    {
        /// <summary>
        /// 保存用户信息
        /// </summary>
        /// <param name="user">用户信息</param>
        /// <param name="isSetApproved">表示是否启用客户</param>
        /// <returns></returns>
        User Save(User user, SysLoggerDto logger, bool isSetApproved = false);
        /// <summary>
        /// 启用用户账号
        /// </summary>
        void Approve(string identity, SysLoggerDto logger);

        /// <summary>
        /// 停用用户账号
        /// <param name="identity">登录名或员工号</param>
        /// </summary>
        void DisApprove(string identity, SysLoggerDto logger);

        /// <summary>
        /// 用户登录名是否唯一
        /// </summary>
        /// <param name="loginName">登录名称</param>
        /// <returns>验证结果</returns>
        bool IsUniqueLoginName(string loginName);

        /// <summary>
        /// 重置用户密码, 密码按特定算法自动生成
        /// <param name="identity">登录名或员工号</param>
        /// </summary>
        void ResetPassword(string identity, SysLoggerDto logger);

        /// <summary>
        /// 更改用户密码, 需提供旧密码进行验证
        /// <param name="identity">登录名或员工号</param>
        /// </summary>
        void ChangePassword(string identity, string oldPassword, string newPassword, SysLoggerDto logger);

        /// <summary>
        /// 更改用户密码, 不验证旧密码，直接改为新密码
        /// </summary>
        void ChangePassword(string identity, string newPassword, SysLoggerDto logger);

        /// <summary>
        /// 根据用户Id获取用户信息
        /// </summary>
        User GetById(string id);


        /// <summary>
        /// 根据用户标识获取用户信息
        /// <param name="identity">登录名或员工号</param>
        /// </summary>
        User GetByIdentity(string identity);

        /// <summary>
        /// 获取用户聚合信息，包括权限与角色
        /// <param name="identity">登录名或员工号</param>
        /// </summary>
        GetUserResponse GetPackageUserInfo(string identity);

        /// <summary>
        /// 创建新用户
        /// </summary>
        User Create(CreateUserRequest request, SysLoggerDto logger);

        /// <summary>
        /// 根据条件查找用户
        /// </summary>
        /// <param name="request">查找请求</param>
        /// <returns>查询响应</returns>
        FindUserResponse FindUsers(FindUserRequest request);

        /// <summary>
        /// 获取用户角色列表
        /// <param name="identity">登录名或员工号</param>
        /// </summary>
        IEnumerable<Role> GetRoles(string identity);

        /// <summary>
        /// 获取用户权限编码
        /// <param name="identity">登录名或员工号</param>
        /// </summary>
        IEnumerable<string> GetOwnedPermissionCodes(string identity);

        /// <summary>
        /// 获取用户的关联菜单
        /// <param name="identity">登录名或员工号</param>
        /// </summary>
        List<Menu> GetUserMenus(string appId, string identity);

        /// <summary>
        /// 获取用户的关联菜单
        /// </summary>
        /// <param name="getAll">是否获取所有应用系统的菜单</param>
        /// <param name="appId">应用Id</param>
        /// <param name="identity">用户标识</param>
        IEnumerable<Menu> GetUserMenus(bool getAll, string appId, string identity);

        /// <summary>
        /// 检查用户是否具有指定的权限
        /// <param name="identity">登录名或员工号</param>
        /// </summary>
        bool HasPermission(string identity, string permissionCode);

        /// <summary>
        /// 检查用户是否拥有指定权限列表的所有权限
        /// <param name="identity">登录名或员工号</param>
        /// </summary>
        bool HasPermissions(string identity, string[] permissionCodes);

        /// <summary>
        /// 检查用户是否拥有指定权限列表中的任一权限
        /// <param name="identity">登录名或员工号</param>
        /// </summary>
        bool HasAnyPermissions(string identity, string[] permissionCodes);

        /// <summary>
        /// 检查用户是否属于指定角色
        /// <param name="identity">登录名或员工号</param>
        /// </summary>
        bool IsInRole(string identity, string roleCode);

        /// <summary>
        /// 检查用户是否属于指定的所有角色列表中
        /// <param name="identity">登录名或员工号</param>
        /// </summary>
        bool IsInAllRoles(string identity, params string[] roleCodes);

        /// <summary>
        /// 获取内部和外部Api账号
        /// </summary>
        /// <returns></returns>
        IEnumerable<User> GetApiUserList();

        /// <summary>
        /// 获取内部和外部Api账号
        /// </summary>
        /// <returns></returns>
        IEnumerable<User> GetList(FindUserRequest request);
    }
}
