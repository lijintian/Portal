using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Caching;
using System.Web.Configuration;
using Portal.SDK.Security.Configuration;
using Microsoft.Win32.SafeHandles;

namespace Portal.SDK.Security
{
    /// <summary>
    /// 表示Portal验证配置
    /// </summary>
    public static class PortalAuthenticationConfig
    {
        private static string _authCookieName;
        private static string _cookieDomain;
        private static string _portalLoginUrl;
        private static int _cacheExpiredTime;
        private static string _cacheName;
        private static string _returnUlr;
        private static string _tokenUrlParameterName;
        private static string _applicationName;
        private static string _portalFrame;
        private static string _portalDefaultUrl;
        private static bool _isFixedRedirect;
        private static string _fixedRedirectUrl;

        private static NameValueCollection _cacheProviers;

        static PortalAuthenticationConfig()
        {
            PortalAuthenticationConfig.Initialize();
        }
        static void Initialize()
        {
            var configGroup = (PortalAuthentication)WebConfigurationManager.OpenWebConfiguration("~").SectionGroups["PortalAuthentication"];
            if (configGroup == null)
            {
                throw new ConfigurationErrorsException("missed portal config section group.");
            }
            var authConfig = configGroup.AuthenticationBase;
            _authCookieName = string.IsNullOrEmpty(authConfig.AuthCookieName) ? ".CK1PortalAuth" : authConfig.AuthCookieName;
            _applicationName = string.IsNullOrEmpty(authConfig.ApplicationName) ? "app" : authConfig.ApplicationName;
            _cookieDomain = authConfig.AuthCookieDomain ?? "";
            _portalLoginUrl = authConfig.LoginUrl;
            _cacheName = authConfig.CacheName;
            _cacheExpiredTime = authConfig.CacheExpiredTime;
            _returnUlr = "returnUrl";
            _tokenUrlParameterName = "token";
            _portalFrame = "portalFrame";
            _portalDefaultUrl = "dUrl";

            var redirectConfig = ConfigurationManager.AppSettings["IsFixedRedirectForPortal"];
            _isFixedRedirect = bool.Parse(redirectConfig ?? "false");
            _fixedRedirectUrl = ConfigurationManager.AppSettings["FixedRedirectForPortal"] ?? "notSetFixedUrl";
            
            var sections = configGroup.CacheProviders.All;
            _cacheProviers = new NameValueCollection();
            for (int i = 0; i < sections.Count; i++)
            {
                _cacheProviers.Add(sections[i].Name, sections[i].Type);
            }

        }

        /// <summary>
        /// portal验证Cookie名称
        /// </summary>
        public static string AuthCookieName
        {
            get
            {
                return PortalAuthenticationConfig._authCookieName;
            }
        }

        /// <summary>
        /// 表示cookie域
        /// </summary>
        public static string CookieDomain
        {
            get
            {
                return PortalAuthenticationConfig._cookieDomain;
            }
        }

        /// <summary>
        /// 表示portal统一登录Url
        /// </summary>
        public static string PortalLoginUrl
        {
            get
            {
                return PortalAuthenticationConfig._portalLoginUrl;
            }
        }


        /// <summary>
        /// 表示用户信息缓存过期时间
        /// </summary>
        public static int CacheExpiredTime
        {
            get
            {
                return _cacheExpiredTime;
            }
        }

        /// <summary>
        /// 表示选用缓存的名称
        /// </summary>
        public static string CacheName
        {
            get
            {
                return _cacheName;
            }
        }

        /// <summary>
        /// 表示返回Url的参数名称
        /// </summary>
        public static string ReturnUrl
        {
            get
            {
                return PortalAuthenticationConfig._returnUlr;
            }
        }

        /// <summary>
        /// 表示token Url参数名称
        /// </summary>
        public static string TokenUrlParameterName
        {
            get
            {
                return PortalAuthenticationConfig._tokenUrlParameterName;
            }
        }

        /// <summary>
        /// 表示从Portal系统中Frame加载的页面
        /// </summary>
        public static string PortalFrameName
        {
            get
            {
                return PortalAuthenticationConfig._portalFrame;
            }
        }

        /// <summary>
        /// 表示从Portal系统默认加载页
        /// </summary>
        public static string PortalDefaultUrl
        {
            get
            {
                return PortalAuthenticationConfig._portalDefaultUrl;
            }
        }

        /// <summary>
        /// 表示应用程序名称
        /// </summary>
        public static string ApplicationName
        {
            get
            {
                return PortalAuthenticationConfig._applicationName;
            }
        }

        public static NameValueCollection CacheProviders
        {
            get
            {
                return _cacheProviers;
            }
        }

        /// <summary>
        /// 表示是否配置固定重定向
        /// </summary>
        public static bool IsFixedRedirect
        {
            get
            {
                return _isFixedRedirect;
            }
        }

        /// <summary>
        /// 固定重定向地址
        /// </summary>
        public static string FixedRedirect
        {
            get
            {
                return _fixedRedirectUrl;
            }
        }
    }
}
