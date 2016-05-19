using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Web;
using Portal.Dto;
using Portal.SDK.Cache;

namespace Portal.SDK.Security
{
    /// <summary>
    /// 表示Portal验证帮助类
    /// </summary>
    public static class PortalAuthenticationHelper
    {
        private static readonly string CacheTokenPrefix = PortalAuthenticationHelper.GenerateTokenPrefix();
        /// <summary>
        /// 设置Portal验证Cookie
        /// </summary>
        public static void SetAuthCookie(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                throw new ArgumentException("token");
            }

            HttpContext.Current.Response.Cookies.Remove(PortalAuthenticationConfig.AuthCookieName);
            var authCookie = new HttpCookie(PortalAuthenticationConfig.AuthCookieName, token)
            {
                HttpOnly = true,
                Domain = PortalAuthenticationConfig.CookieDomain
            };
            HttpContext.Current.Response.Cookies.Add(authCookie);
        }

        /// <summary>
        /// 登录成功后处理，包括设置验证cookie, 保存用户信息至缓存等
        /// </summary>
        public static PortalPrincipal LoginedIn(UserPackageInfo userInfo)
        {
            if (userInfo == null)
            {
                throw new ArgumentException("userinfo");
            }
            var principal = new PortalPrincipal(userInfo);
            PortalAuthenticationHelper.SetAuthCookie(userInfo.Token);
            CacheManager.Set(CacheTokenPrefix + userInfo.Token, principal, PortalAuthenticationConfig.CacheExpiredTime);

            return principal;
        }

        /// <summary>
        /// 获取缓存的用户主体
        /// </summary>
        public static PortalPrincipal GetPrincipalFromCache(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                throw new ArgumentException("token");
            }
            return CacheManager.Get<PortalPrincipal>(CacheTokenPrefix + token);
        }

        /// <summary>
        /// 注销处理
        /// </summary>
        public static void LoginOut(string returnUrl, Action beforeRedirectCallback)
        {
            //remove auth cookie
            var httpRequest = HttpContext.Current.Request;
            var httpResponse = HttpContext.Current.Response;
            var cookie = httpRequest.Cookies[PortalAuthenticationConfig.AuthCookieName];
            if (cookie != null)
            {
                cookie.Value = null;
                cookie.Values.Clear();
                cookie.Expires = DateTime.Now.AddMonths(-1);
                cookie.Domain = PortalAuthenticationConfig.CookieDomain;

                httpResponse.Cookies.Remove(PortalAuthenticationConfig.AuthCookieName);
                httpResponse.Cookies.Add(cookie);
            }

            //clear cache
            var user = HttpContext.Current.User as PortalPrincipal;
            if (user != null)
            {
                CacheManager.Remove(user.Token);
            }

            if (beforeRedirectCallback != null)
            {
                beforeRedirectCallback();
            }

            var redirectUrl = string.IsNullOrEmpty(returnUrl) ? PortalAuthenticationConfig.PortalLoginUrl : returnUrl;
            httpResponse.Redirect(redirectUrl);
        }

        /// <summary>
        /// 检查访问的url是否Portal
        /// </summary>
        public static bool IsAccessingPortalHomeOrLoginPage(string accessUrl)
        {
            return PortalAuthenticationHelper.IsAccessingPortalHomeOrLoginPage(new Uri(accessUrl));
        }

        public static bool IsAccessingPortalHomeOrLoginPage(Uri accessUri)
        {
            var loginUri = new Uri(PortalAuthenticationConfig.PortalLoginUrl);
            bool isAccessingPortal = string.Compare(loginUri.Host, accessUri.Host, true) == 0;

            if (isAccessingPortal)
            {
                var accessingPathAndQuery = accessUri.GetComponents(UriComponents.PathAndQuery, UriFormat.Unescaped);
                if (accessingPathAndQuery == "/")
                {
                    //home page
                    return true;
                }

                var loginPathAndQuery = loginUri.GetComponents(UriComponents.PathAndQuery, UriFormat.Unescaped);
                if (accessingPathAndQuery.StartsWith(loginPathAndQuery, StringComparison.CurrentCultureIgnoreCase))
                {
                    //login page
                    return true;
                }
            }
            
            return false;
        }

        private static string GenerateTokenPrefix()
        {
            var random = new Random();
            return "#" + PortalAuthenticationConfig.ApplicationName + random.Next();
        }
    }
}
