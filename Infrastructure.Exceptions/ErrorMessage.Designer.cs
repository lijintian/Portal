﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.42000
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

namespace Portal.Infrastructure.Exceptions {
    using System;
    
    
    /// <summary>
    ///   一个强类型的资源类，用于查找本地化的字符串等。
    /// </summary>
    // 此类是由 StronglyTypedResourceBuilder
    // 类通过类似于 ResGen 或 Visual Studio 的工具自动生成的。
    // 若要添加或移除成员，请编辑 .ResX 文件，然后重新运行 ResGen
    // (以 /str 作为命令选项)，或重新生成 VS 项目。
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class ErrorMessage {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal ErrorMessage() {
        }
        
        /// <summary>
        ///   返回此类使用的缓存的 ResourceManager 实例。
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Portal.Infrastructure.Exceptions.ErrorMessage", typeof(ErrorMessage).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   使用此强类型资源类，为所有资源查找
        ///   重写当前线程的 CurrentUICulture 属性。
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   查找类似 Access denied 的本地化字符串。
        /// </summary>
        public static string AccessDenied {
            get {
                return ResourceManager.GetString("AccessDenied", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 请求的Token不存在 的本地化字符串。
        /// </summary>
        public static string AccessTokenNotExist {
            get {
                return ResourceManager.GetString("AccessTokenNotExist", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 访问Token验证失败，请检查是否禁用或过期,权限码是否匹配 的本地化字符串。
        /// </summary>
        public static string AccessTokenValidateFail {
            get {
                return ResourceManager.GetString("AccessTokenValidateFail", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 当前应用不存在该菜单ID:{0} 的本地化字符串。
        /// </summary>
        public static string ApplicationNoFoundMenuId {
            get {
                return ResourceManager.GetString("ApplicationNoFoundMenuId", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 你的应用处于无效状态. 的本地化字符串。
        /// </summary>
        public static string ApplicationStateError {
            get {
                return ResourceManager.GetString("ApplicationStateError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 当前应用不存在权限代码:{0} 的本地化字符串。
        /// </summary>
        public static string AppNoFoundPermissionCode {
            get {
                return ResourceManager.GetString("AppNoFoundPermissionCode", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 {0}不存在 的本地化字符串。
        /// </summary>
        public static string ArgumentCannotBeNull {
            get {
                return ResourceManager.GetString("ArgumentCannotBeNull", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 {0}不能为空 的本地化字符串。
        /// </summary>
        public static string ArgumentCannotBeNullOrEmptyString {
            get {
                return ResourceManager.GetString("ArgumentCannotBeNullOrEmptyString", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 授权码已过期或已使用，不能设置为启用 的本地化字符串。
        /// </summary>
        public static string AuthorizationCodeCannotSetEnable {
            get {
                return ResourceManager.GetString("AuthorizationCodeCannotSetEnable", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 当前用户已经为禁用状态，不能再次转为禁用状态 的本地化字符串。
        /// </summary>
        public static string CannotChangeUserStateToDisabled {
            get {
                return ResourceManager.GetString("CannotChangeUserStateToDisabled", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 当前用户已经为启用状态，不能再次转为启用状态 的本地化字符串。
        /// </summary>
        public static string CannotChangeUserStateToEnabled {
            get {
                return ResourceManager.GetString("CannotChangeUserStateToEnabled", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 请求授权的应用不存在 的本地化字符串。
        /// </summary>
        public static string CustomerAuthorizationAppNotExist {
            get {
                return ResourceManager.GetString("CustomerAuthorizationAppNotExist", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 未指定应用Clientid参数 的本地化字符串。
        /// </summary>
        public static string CustomerAuthorizationClientIdMissing {
            get {
                return ResourceManager.GetString("CustomerAuthorizationClientIdMissing", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 未指定重定向参数(redirect_uri) 的本地化字符串。
        /// </summary>
        public static string CustomerAuthorizationRedirectUriMissing {
            get {
                return ResourceManager.GetString("CustomerAuthorizationRedirectUriMissing", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 请求授权应用的重定向地址与应用注册时回调地址非同源 的本地化字符串。
        /// </summary>
        public static string CustomerAuthorizationRedirectUrlIsNotCognatedWithRegisterCallbcakUrl {
            get {
                return ResourceManager.GetString("CustomerAuthorizationRedirectUrlIsNotCognatedWithRegisterCallbcakUrl", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 未指定请求授权响应类型(responseType) 的本地化字符串。
        /// </summary>
        public static string CustomerAuthorizationResponseTypeMissing {
            get {
                return ResourceManager.GetString("CustomerAuthorizationResponseTypeMissing", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 未指定应用授权范围参数(scope) 的本地化字符串。
        /// </summary>
        public static string CustomerAuthorizationScopeMissing {
            get {
                return ResourceManager.GetString("CustomerAuthorizationScopeMissing", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 未指定授权范围参数 的本地化字符串。
        /// </summary>
        public static string CustomerAuthorizationScopeMustHasVal {
            get {
                return ResourceManager.GetString("CustomerAuthorizationScopeMustHasVal", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 授权范围与应用授权不匹配，请重新确认授权范围(scope) 的本地化字符串。
        /// </summary>
        public static string CustomerAuthorizationScopeNotMatch {
            get {
                return ResourceManager.GetString("CustomerAuthorizationScopeNotMatch", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 请求的授权部分不存在，请重新确认授权范围 的本地化字符串。
        /// </summary>
        public static string CustomerAuthorizationScopePartNotExist {
            get {
                return ResourceManager.GetString("CustomerAuthorizationScopePartNotExist", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 不支持的请求授权类型 的本地化字符串。
        /// </summary>
        public static string CustomerAuthorizationUnsupportType {
            get {
                return ResourceManager.GetString("CustomerAuthorizationUnsupportType", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 客户账号已处于失效状态，授权终止 的本地化字符串。
        /// </summary>
        public static string CustomerIsForbiddenAccessTokenFail {
            get {
                return ResourceManager.GetString("CustomerIsForbiddenAccessTokenFail", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 开发者应用程序回调地址不能为空 的本地化字符串。
        /// </summary>
        public static string DeveloperApplicatinCallbackUrlCannotBeNull {
            get {
                return ResourceManager.GetString("DeveloperApplicatinCallbackUrlCannotBeNull", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 开发者应用程序不是审核通过，可能在非法的状态下提审 的本地化字符串。
        /// </summary>
        public static string DeveloperApplicatinInvalidStateCannotBeApproved {
            get {
                return ResourceManager.GetString("DeveloperApplicatinInvalidStateCannotBeApproved", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 开发者应用程序在非开发中状态不能提交审核 的本地化字符串。
        /// </summary>
        public static string DeveloperApplicatinInvalidStateCannotSubmitToApprove {
            get {
                return ResourceManager.GetString("DeveloperApplicatinInvalidStateCannotSubmitToApprove", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 开发者应用程序在非开发中状态不能修改信息 的本地化字符串。
        /// </summary>
        public static string DeveloperApplicatinInvalidStateCannotUpdate {
            get {
                return ResourceManager.GetString("DeveloperApplicatinInvalidStateCannotUpdate", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 开发者应用程序在非开发中状态不能更新API权限信息 的本地化字符串。
        /// </summary>
        public static string DeveloperApplicatinInvalidStateCannotUpdatePermission {
            get {
                return ResourceManager.GetString("DeveloperApplicatinInvalidStateCannotUpdatePermission", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 开发者应用程序在当前的状态下不能驳回 的本地化字符串。
        /// </summary>
        public static string DeveloperApplicatinInvalidStateToBeReject {
            get {
                return ResourceManager.GetString("DeveloperApplicatinInvalidStateToBeReject", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 开发者应用程序名称不能为空 的本地化字符串。
        /// </summary>
        public static string DeveloperApplicatinNameCannotBeNull {
            get {
                return ResourceManager.GetString("DeveloperApplicatinNameCannotBeNull", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 开发者应用程序选择的权限码不存在 的本地化字符串。
        /// </summary>
        public static string DeveloperApplicatinNoFoundCode {
            get {
                return ResourceManager.GetString("DeveloperApplicatinNoFoundCode", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 请求的开发者应用不存在 的本地化字符串。
        /// </summary>
        public static string DeveloperApplicatinUnExists {
            get {
                return ResourceManager.GetString("DeveloperApplicatinUnExists", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 开发者应用程序没有选择产品API，不能提交审核 的本地化字符串。
        /// </summary>
        public static string DeveloperApplicatinUnGroupCannotSubmitToApprove {
            get {
                return ResourceManager.GetString("DeveloperApplicatinUnGroupCannotSubmitToApprove", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 开发者应用程序所属账号不能为空 的本地化字符串。
        /// </summary>
        public static string DeveloperApplicatinUserCannotBeNull {
            get {
                return ResourceManager.GetString("DeveloperApplicatinUserCannotBeNull", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 邮箱不允许为空 的本地化字符串。
        /// </summary>
        public static string EmailCannotBeNull {
            get {
                return ResourceManager.GetString("EmailCannotBeNull", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 已存在相同的分组编码 的本地化字符串。
        /// </summary>
        public static string ExistsSameApiPermissionGroupCode {
            get {
                return ResourceManager.GetString("ExistsSameApiPermissionGroupCode", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 已存在相同的分组名称 的本地化字符串。
        /// </summary>
        public static string ExistsSameApiPermissionGroupName {
            get {
                return ResourceManager.GetString("ExistsSameApiPermissionGroupName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 系统已存在相同的应用英文名称:{0} 的本地化字符串。
        /// </summary>
        public static string ExistsSameApplicationEnName {
            get {
                return ResourceManager.GetString("ExistsSameApplicationEnName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 系统已存在相同的应用名称:{0} 的本地化字符串。
        /// </summary>
        public static string ExistsSameApplicationName {
            get {
                return ResourceManager.GetString("ExistsSameApplicationName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 已存在相同的开发者应用程序名称:{0} 的本地化字符串。
        /// </summary>
        public static string ExistsSameDeveloperAppName {
            get {
                return ResourceManager.GetString("ExistsSameDeveloperAppName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 系统已存在相同的员工号:{0} 的本地化字符串。
        /// </summary>
        public static string ExistsSameEmployeeNo {
            get {
                return ResourceManager.GetString("ExistsSameEmployeeNo", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 已存在相同的权限编码:{0} 的本地化字符串。
        /// </summary>
        public static string ExistsSamePermissionCode {
            get {
                return ResourceManager.GetString("ExistsSamePermissionCode", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 已存在相同的权限名称 的本地化字符串。
        /// </summary>
        public static string ExistsSamePermissionName {
            get {
                return ResourceManager.GetString("ExistsSamePermissionName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 已存在相同的角色代码 的本地化字符串。
        /// </summary>
        public static string ExistsSameRoleCode {
            get {
                return ResourceManager.GetString("ExistsSameRoleCode", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 已存在相同的角色名称 的本地化字符串。
        /// </summary>
        public static string ExistsSameRoleName {
            get {
                return ResourceManager.GetString("ExistsSameRoleName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 系统已存在相同的用户名:{0} 的本地化字符串。
        /// </summary>
        public static string ExistsSameUserLoginName {
            get {
                return ResourceManager.GetString("ExistsSameUserLoginName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 用户名不允许为空 的本地化字符串。
        /// </summary>
        public static string IdentityCannotBeNull {
            get {
                return ResourceManager.GetString("IdentityCannotBeNull", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 用户{0}不存在 的本地化字符串。
        /// </summary>
        public static string IdentityNotExist {
            get {
                return ResourceManager.GetString("IdentityNotExist", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 Internal error 的本地化字符串。
        /// </summary>
        public static string InternalError {
            get {
                return ResourceManager.GetString("InternalError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 无效的Email 的本地化字符串。
        /// </summary>
        public static string InvalidEmail {
            get {
                return ResourceManager.GetString("InvalidEmail", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 无效的手机号码 的本地化字符串。
        /// </summary>
        public static string InvalidPhone {
            get {
                return ResourceManager.GetString("InvalidPhone", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 需要客户授权的权限代码:{0} 的本地化字符串。
        /// </summary>
        public static string IsCustomerGrantedPermissionCode {
            get {
                return ResourceManager.GetString("IsCustomerGrantedPermissionCode", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 需要对外开放的权限码:{0} 的本地化字符串。
        /// </summary>
        public static string IsOpenedPermissionCode {
            get {
                return ResourceManager.GetString("IsOpenedPermissionCode", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 Login failed 的本地化字符串。
        /// </summary>
        public static string LoginFailed {
            get {
                return ResourceManager.GetString("LoginFailed", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 登陆名称不允许为空 的本地化字符串。
        /// </summary>
        public static string LoginNameCannotBeNull {
            get {
                return ResourceManager.GetString("LoginNameCannotBeNull", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 当前用户已被禁用 的本地化字符串。
        /// </summary>
        public static string LoginNameIsApproved {
            get {
                return ResourceManager.GetString("LoginNameIsApproved", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 用户名或密码无效 的本地化字符串。
        /// </summary>
        public static string LoginNameOrPasswordInvalid {
            get {
                return ResourceManager.GetString("LoginNameOrPasswordInvalid", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 菜单ID:{0}存在子菜单，不允许删除 的本地化字符串。
        /// </summary>
        public static string MenuHasChild {
            get {
                return ResourceManager.GetString("MenuHasChild", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 不存在API权限分组代码:{0} 的本地化字符串。
        /// </summary>
        public static string NoFoundApiPermissionGroupCode {
            get {
                return ResourceManager.GetString("NoFoundApiPermissionGroupCode", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 系统不存在此应用 的本地化字符串。
        /// </summary>
        public static string NoFoundApplication {
            get {
                return ResourceManager.GetString("NoFoundApplication", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 不存在该菜单ID:{0} 的本地化字符串。
        /// </summary>
        public static string NoFoundMenuId {
            get {
                return ResourceManager.GetString("NoFoundMenuId", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 系统不存在权限代码:{0} 的本地化字符串。
        /// </summary>
        public static string NoFoundPermissionCode {
            get {
                return ResourceManager.GetString("NoFoundPermissionCode", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 系统不存在角色代码:{0} 的本地化字符串。
        /// </summary>
        public static string NoFoundRoleCode {
            get {
                return ResourceManager.GetString("NoFoundRoleCode", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 旧密码不匹配 的本地化字符串。
        /// </summary>
        public static string OldPassowrdNotMatch {
            get {
                return ResourceManager.GetString("OldPassowrdNotMatch", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 密码不允许为空 的本地化字符串。
        /// </summary>
        public static string PasswordCannotBeNull {
            get {
                return ResourceManager.GetString("PasswordCannotBeNull", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 请指定要验证的权限码. 的本地化字符串。
        /// </summary>
        public static string PermissionCodeCannotNull {
            get {
                return ResourceManager.GetString("PermissionCodeCannotNull", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 请求访问的授权码不属于当前应用 的本地化字符串。
        /// </summary>
        public static string RequestAccessTokenAuthorizationCodeAppNotMatch {
            get {
                return ResourceManager.GetString("RequestAccessTokenAuthorizationCodeAppNotMatch", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 请求访问的授权码无效. 的本地化字符串。
        /// </summary>
        public static string RequestAccessTokenAuthorizationCodeError {
            get {
                return ResourceManager.GetString("RequestAccessTokenAuthorizationCodeError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 请求访问Token授权未指定授权码. 的本地化字符串。
        /// </summary>
        public static string RequestAccessTokenAuthorizationCodeMissing {
            get {
                return ResourceManager.GetString("RequestAccessTokenAuthorizationCodeMissing", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 请求访问的授权码不存在. 的本地化字符串。
        /// </summary>
        public static string RequestAccessTokenAuthorizationCodeNotExist {
            get {
                return ResourceManager.GetString("RequestAccessTokenAuthorizationCodeNotExist", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 请求访问Token授权刷新Token无效 的本地化字符串。
        /// </summary>
        public static string RequestAccessTokenAuthorizationRefreshTokenError {
            get {
                return ResourceManager.GetString("RequestAccessTokenAuthorizationRefreshTokenError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 请求访问Token授权未指定刷新Token 的本地化字符串。
        /// </summary>
        public static string RequestAccessTokenAuthorizationRefreshTokenMissing {
            get {
                return ResourceManager.GetString("RequestAccessTokenAuthorizationRefreshTokenMissing", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 请求访问Token授权刷新Token不存在 的本地化字符串。
        /// </summary>
        public static string RequestAccessTokenAuthorizationRefreshTokenNotExist {
            get {
                return ResourceManager.GetString("RequestAccessTokenAuthorizationRefreshTokenNotExist", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 请求访问Token授权刷新Token不属于当前应用 的本地化字符串。
        /// </summary>
        public static string RequestAccessTokenAuthorizationRefreshTokenNotMatch {
            get {
                return ResourceManager.GetString("RequestAccessTokenAuthorizationRefreshTokenNotMatch", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 请求的重定向地址格式不正确 的本地化字符串。
        /// </summary>
        public static string RequestAccessTokenCallbackInvalidFormat {
            get {
                return ResourceManager.GetString("RequestAccessTokenCallbackInvalidFormat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 请求访问Token授权类型指定错误，目前支持authorization_code和refresh_token两种方式 的本地化字符串。
        /// </summary>
        public static string RequestAccessTokenGrantTypeError {
            get {
                return ResourceManager.GetString("RequestAccessTokenGrantTypeError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 请求访问Token授权类型未指定.(Grant_type) 的本地化字符串。
        /// </summary>
        public static string RequestAccessTokenGrantTypeMissing {
            get {
                return ResourceManager.GetString("RequestAccessTokenGrantTypeMissing", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 请求访问Token应用密钥参数未指定.(Client_secret) 的本地化字符串。
        /// </summary>
        public static string RequestAccessTokenSecretMissing {
            get {
                return ResourceManager.GetString("RequestAccessTokenSecretMissing", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 请求访问Token应用密钥不匹配.(Client_secret) 的本地化字符串。
        /// </summary>
        public static string RequestAccessTokenSecretNotMatch {
            get {
                return ResourceManager.GetString("RequestAccessTokenSecretNotMatch", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 调用来源不允许为空 的本地化字符串。
        /// </summary>
        public static string SourceCannotBeNull {
            get {
                return ResourceManager.GetString("SourceCannotBeNull", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 请指定要验证的Token. 的本地化字符串。
        /// </summary>
        public static string TokenCannotBeNull {
            get {
                return ResourceManager.GetString("TokenCannotBeNull", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 Token已过期，不能设置为启用 的本地化字符串。
        /// </summary>
        public static string TokenExpiredCannotSetEnable {
            get {
                return ResourceManager.GetString("TokenExpiredCannotSetEnable", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 不支持的权限类型 的本地化字符串。
        /// </summary>
        public static string UnSupportPermissionType {
            get {
                return ResourceManager.GetString("UnSupportPermissionType", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 你的账号已禁用. 的本地化字符串。
        /// </summary>
        public static string UserAccountDisable {
            get {
                return ResourceManager.GetString("UserAccountDisable", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 The request parameter can not null 的本地化字符串。
        /// </summary>
        public static string ValidationRequestParameterNull {
            get {
                return ResourceManager.GetString("ValidationRequestParameterNull", resourceCulture);
            }
        }
    }
}
