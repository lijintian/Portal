﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.42000
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

namespace Portal.Domain {
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
    public class Resource {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resource() {
        }
        
        /// <summary>
        ///   返回此类使用的缓存的 ResourceManager 实例。
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Portal.Domain.Resource", typeof(Resource).Assembly);
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
        ///   查找类似 访问Token不能为空 的本地化字符串。
        /// </summary>
        public static string AccessTokenCannotBeNull {
            get {
                return ResourceManager.GetString("AccessTokenCannotBeNull", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 访问Token不存在 的本地化字符串。
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
        ///   查找类似 生成Token时必须指定API权限 的本地化字符串。
        /// </summary>
        public static string ApiPermissionMustNotNull {
            get {
                return ResourceManager.GetString("ApiPermissionMustNotNull", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 授权码不能为空 的本地化字符串。
        /// </summary>
        public static string AuthorizationCodeCannotBeNull {
            get {
                return ResourceManager.GetString("AuthorizationCodeCannotBeNull", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 回调地址格式不正确，例如https://www.xxx.com/back 的本地化字符串。
        /// </summary>
        public static string CallbackUrlFormatError {
            get {
                return ResourceManager.GetString("CallbackUrlFormatError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 创建Token的权限码有声明客户授权但未提供客户标识. 的本地化字符串。
        /// </summary>
        public static string CreateTokenNotProviderClientIdentity {
            get {
                return ResourceManager.GetString("CreateTokenNotProviderClientIdentity", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 授权客户标识不能为空 的本地化字符串。
        /// </summary>
        public static string CustomerIdentityCannotBeNull {
            get {
                return ResourceManager.GetString("CustomerIdentityCannotBeNull", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 开发者应用标识不能为空 的本地化字符串。
        /// </summary>
        public static string DeveloperAppClientIdCannotBeNull {
            get {
                return ResourceManager.GetString("DeveloperAppClientIdCannotBeNull", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 开发应用程序回调地址不能为空 的本地化字符串。
        /// </summary>
        public static string DeveloperApplicatinCallbackUrlCannotBeNull {
            get {
                return ResourceManager.GetString("DeveloperApplicatinCallbackUrlCannotBeNull", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 开发应用程序名称不能为空 的本地化字符串。
        /// </summary>
        public static string DeveloperApplicatinNameCannotBeNull {
            get {
                return ResourceManager.GetString("DeveloperApplicatinNameCannotBeNull", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 开发应用程序所属账号不能为空 的本地化字符串。
        /// </summary>
        public static string DeveloperApplicatinUserCannotBeNull {
            get {
                return ResourceManager.GetString("DeveloperApplicatinUserCannotBeNull", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 验证的Token权限必须提供客户标识 的本地化字符串。
        /// </summary>
        public static string NotProviderClientIdentityForTestToken {
            get {
                return ResourceManager.GetString("NotProviderClientIdentityForTestToken", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 权限码不能为空. 的本地化字符串。
        /// </summary>
        public static string PermissionCodeCannotNull {
            get {
                return ResourceManager.GetString("PermissionCodeCannotNull", resourceCulture);
            }
        }
    }
}
