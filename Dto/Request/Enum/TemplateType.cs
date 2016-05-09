using System.ComponentModel;

namespace Portal.Dto.Request.Enum
{
    public enum TemplateType
    {
        /// <summary>
        /// 批量新增用户角色
        /// </summary>
        [Description("批量新增用户角色")]
        CreateUserRole = 1,

        /// <summary>
        /// 批量删除用户角色
        /// </summary>
        [Description("批量删除用户角色")]
        DeleteUserRole = 2,

        /// <summary>
        /// 批量新增用户权限
        /// </summary>
        [Description("批量新增用户权限")]
        CreateUserPermission = 3,

        /// <summary>
        ///  批量删除用户权限
        /// </summary>
        [Description("批量删除用户权限")]
        DeleteUserPermission = 4,

        /// <summary>
        /// 批量导入角色
        /// </summary>
        [Description("批量导入角色")]
        ImportRole = 5,

        /// <summary>
        /// 批量新增角色权限
        /// </summary>
        [Description("批量新增角色权限")]
        CreateRolePermission = 6,

        /// <summary>
        ///  批量删除角色权限
        /// </summary>
        [Description("批量删除角色权限")]
        DeleteRolePermission = 7,

        /// <summary>
        /// 批量导入权限
        /// </summary>
        [Description("批量导入权限")]
        ImportPermission = 8,

        /// <summary>
        /// 批量导入菜单
        /// </summary>
        [Description("批量导入菜单")]
        ImportMenu = 9,

        /// <summary>
        /// 批量新增客户角色
        /// </summary>
        [Description("批量新增客户角色")]
        CreateCustomerRole = 10,

        /// <summary>
        /// 批量删除客户角色
        /// </summary>
        [Description("批量删除客户角色")]
        DeleteCustomerRole = 11,

        /// <summary>
        /// 批量新增用户权限
        /// </summary>
        [Description("批量新增客户权限")]
        CreateCustomerPermission = 12,

        /// <summary>
        ///  批量删除客户权限
        /// </summary>
        [Description("批量删除客户权限")]
        DeleteCustomerPermission = 13,
    }
}
