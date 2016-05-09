using System;

namespace Portal.Web.Core
{	
	public static class PermissionCodes
    {
			/// <summary>
			/// 应用系统管理页面 查询
			/// </summary>
			public const string Application = "Application";
			/// <summary>
			/// 应用系统管理页面 新增
			/// </summary>
			public const string Application_Add = "Application_Add";
			/// <summary>
			/// 应用系统管理页面 编辑
			/// </summary>
			public const string Application_Edit = "Application_Edit";
			/// <summary>
			/// 角色管理页面 查询
			/// </summary>
			public const string Role = "Role";
			/// <summary>
			/// 角色管理页面 新增
			/// </summary>
			public const string Role_Add = "Role_Add";
			/// <summary>
			/// 角色管理页面 编辑
			/// </summary>
			public const string Role_Edit = "Role_Edit";
			/// <summary>
			/// 角色管理页面 设置角色的权限
			/// </summary>
			public const string Set_Role_Permission = "Set_Role_Permission";
			/// <summary>
			/// 批量导入角色
			/// </summary>
			public const string ImportRole = "ImportRole";
			/// <summary>
			/// 批量新增角色权限
			/// </summary>
			public const string CreateRolePermission = "CreateRolePermission";
			/// <summary>
			/// 批量删除角色权限
			/// </summary>
			public const string DeleteRolePermission = "DeleteRolePermission";
			/// <summary>
			/// 用户页面 查询
			/// </summary>
			public const string User = "User";
			/// <summary>
			/// 用户启用
			/// </summary>
			public const string User_Approve = "User_Approve";
			/// <summary>
			/// 用户禁用
			/// </summary>
			public const string User_DisApprove = "User_DisApprove";
			/// <summary>
			/// 用户设置角色权限
			/// </summary>
			public const string User_SetRole = "User_SetRole";
			/// <summary>
			/// 用户设置权限
			/// </summary>
			public const string User_SetPermission = "User_SetPermission";
			/// <summary>
			/// 批量新增用户角色
			/// </summary>
			public const string CreateUserRole = "CreateUserRole";
			/// <summary>
			/// 批量删除用户角色
			/// </summary>
			public const string DeleteUserRole = "DeleteUserRole";
			/// <summary>
			/// 批量新增用户权限
			/// </summary>
			public const string CreateUserPermission = "CreateUserPermission";
			/// <summary>
			/// 批量删除用户权限
			/// </summary>
			public const string DeleteUserPermission = "DeleteUserPermission";
			/// <summary>
			/// 客户帐号页面 查询
			/// </summary>
			public const string Customer = "Customer";
			/// <summary>
			/// 客户帐号启用
			/// </summary>
			public const string Customer_Approve  = "Customer_Approve";
			/// <summary>
			/// 客户帐号禁用
			/// </summary>
			public const string Customer_DisApprove = "Customer_DisApprove";
			/// <summary>
			/// 客户帐号设置角色权限
			/// </summary>
			public const string Customer_SetRole = "Customer_SetRole";
			/// <summary>
			/// 客户帐号设置权限
			/// </summary>
			public const string Customer_SetPermission = "Customer_SetPermission";
			/// <summary>
			/// 批量新增客户角色
			/// </summary>
			public const string CreateCustomerRole = "CreateCustomerRole";
			/// <summary>
			/// 批量删除客户角色
			/// </summary>
			public const string DeleteCustomerRole = "DeleteCustomerRole";
			/// <summary>
			/// 批量新增客户权限
			/// </summary>
			public const string CreateCustomerPermission = "CreateCustomerPermission";
			/// <summary>
			/// 批量删除客户权限
			/// </summary>
			public const string DeleteCustomerPermission = "DeleteCustomerPermission";
			/// <summary>
			/// API帐号页面 查询
			/// </summary>
			public const string APIUser = "APIUser";
			/// <summary>
			/// API帐号添加
			/// </summary>
			public const string APIUser_Add = "APIUser_Add";
			/// <summary>
			/// API帐号更新
			/// </summary>
			public const string APIUser_Update = "APIUser_Update";
			/// <summary>
			/// API帐号启用
			/// </summary>
			public const string APIUser_Approve = "APIUser_Approve";
			/// <summary>
			/// API帐号禁用
			/// </summary>
			public const string APIUser_DisApprove = "APIUser_DisApprove";
			/// <summary>
			/// API帐号重置密码
			/// </summary>
			public const string APIUser_ResetPassword = "APIUser_ResetPassword";
			/// <summary>
			/// API帐号设置角色权限
			/// </summary>
			public const string APIUser_SetRole = "APIUser_SetRole";
			/// <summary>
			/// API帐号设置权限
			/// </summary>
			public const string APIUser_SetPermission = "APIUser_SetPermission";
			/// <summary>
			/// 菜单页面 查询
			/// </summary>
			public const string Menu = "Menu";
			/// <summary>
			/// 菜单添加
			/// </summary>
			public const string Menu_Add = "Menu_Add";
			/// <summary>
			/// 菜单更新
			/// </summary>
			public const string Menu_Update = "Menu_Update";
			/// <summary>
			/// 菜单删除
			/// </summary>
			public const string Menu_Delete = "Menu_Delete";
			/// <summary>
			/// 批量导入菜单
			/// </summary>
			public const string ImportMenu = "ImportMenu";
			/// <summary>
			/// 系统权限页面 查询
			/// </summary>
			public const string Permission = "Permission";
			/// <summary>
			/// 系统权限页面添加
			/// </summary>
			public const string Permission_Add = "Permission_Add";
			/// <summary>
			/// 系统权限页面更新
			/// </summary>
			public const string Permission_Update = "Permission_Update";
			/// <summary>
			/// 系统权限页面添加明细
			/// </summary>
			public const string Permission_AddDetail = "Permission_AddDetail";
			/// <summary>
			/// 批量导入权限
			/// </summary>
			public const string ImportPermission = "ImportPermission";
			/// <summary>
			/// API权限页面 查询
			/// </summary>
			public const string APIPermission = "APIPermission";
			/// <summary>
			/// API权限添加
			/// </summary>
			public const string APIPermission_Add = "APIPermission_Add";
			/// <summary>
			/// API权限更新
			/// </summary>
			public const string APIPermission_Update = "APIPermission_Update";
			/// <summary>
			/// 外部应用系统管理 查询
			/// </summary>
			public const string DeveloperApp = "DeveloperApp";
			/// <summary>
			/// 外部应用系统管理 查看
			/// </summary>
			public const string DeveloperApp_View = "DeveloperApp_View";
			/// <summary>
			/// 外部应用系统管理 更新
			/// </summary>
			public const string DeveloperApp_Update = "DeveloperApp_Update";
			/// <summary>
			/// 外部应用系统管理 审核
			/// </summary>
			public const string DeveloperApp_Audit = "DeveloperApp_Audit";
			/// <summary>
			/// 外部应用系统管理 Token
			/// </summary>
			public const string DeveloperApp_Token = "DeveloperApp_Token";
			/// <summary>
			/// 外部应用系统管理 授权码
			/// </summary>
			public const string DeveloperApp_AuthCode = "DeveloperApp_AuthCode";
			/// <summary>
			/// 管理访问Token 查询
			/// </summary>
			public const string Token = "Token";
			/// <summary>
			/// 管理访问Token 启用
			/// </summary>
			public const string Token_Approve = "Token_Approve";
			/// <summary>
			/// 管理访问Token 禁用
			/// </summary>
			public const string Token_DisApprove = "Token_DisApprove";
			/// <summary>
			/// 管理授权码 查询
			/// </summary>
			public const string AuthorizationCode = "AuthorizationCode";
			/// <summary>
			/// API权限分组管理页面 查询
			/// </summary>
			public const string ApiPermissionGroup = "ApiPermissionGroup";
			/// <summary>
			/// API权限分组管理页面 新增
			/// </summary>
			public const string ApiPermissionGroup_Add = "ApiPermissionGroup_Add";
			/// <summary>
			/// API权限分组管理页面 编辑
			/// </summary>
			public const string ApiPermissionGroup_Edit = "ApiPermissionGroup_Edit";
			/// <summary>
			/// API权限分组管理页面 设置权限
			/// </summary>
			public const string Set_ApiPermissionGroup_Permission = "Set_ApiPermissionGroup_Permission";
			/// <summary>
			/// API权限分组管理页面 查看权限
			/// </summary>
			public const string View_ApiPermissionGroup_Permission = "View_ApiPermissionGroup_Permission";
			/// <summary>
			/// 系统日志管理 查询
			/// </summary>
			public const string SysLogger = "SysLogger";
			/// <summary>
			/// 系统日志管理 查看
			/// </summary>
			public const string SysLogger_View = "SysLogger_View";
			/// <summary>
			/// 系统日志管理 创建
			/// </summary>
			public const string SysLogger_Create = "SysLogger_Create";
			/// <summary>
			/// 系统日志管理 更新
			/// </summary>
			public const string SysLogger_Update = "SysLogger_Update";
			/// <summary>
			/// 系统日志管理 删除
			/// </summary>
			public const string SysLogger_Delete = "SysLogger_Delete";
			/// <summary>
			/// 系统日志管理 提交审核
			/// </summary>
			public const string SysLogger_Audit = "SysLogger_Audit";
		
  }
}