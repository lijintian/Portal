using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Policy;
using System.Text;
using System.Web;
using System.Configuration;
using System.Web.Handlers;
using System.Xml.Serialization;
using Portal.Dto;
using Portal.SDK;
using Portal.SDK.Cache;
using Portal.SDK.ServiceClient;
using RestSharp;
using HttpCookie = System.Web.HttpCookie;

namespace Portal.SDK.Security
{
    /// <summary>
    /// 表示Portal验证的http模块
    /// </summary>
    public class PortalAuthenticationModule : IHttpModule
    {
        private static readonly bool Ck1PortalAuthenticationEnable;
        private static readonly List<string> IgnoreExtensions = new List<string>(){ "css", "js", "ico", 
            "jpg", "jpeg", "bmp", "gif", "pic", "png", "tif", "wav", "mp3", "map"};
        private const string WWWAuthenticate = "WWW-Authenticate";
        private bool _isOnEnterCalled;

        static PortalAuthenticationModule()
        {
            var authenticationEnable = ConfigurationManager.AppSettings["CK1PortalAuthenticationEnable"];
            PortalAuthenticationModule.Ck1PortalAuthenticationEnable = bool.Parse(authenticationEnable ?? "false");
        }

        public void Dispose()
        {

        }

        public void Init(HttpApplication context)
        {
            if (PortalAuthenticationModule.Ck1PortalAuthenticationEnable)
            {
                context.AuthenticateRequest += new EventHandler(this.OnEnter);
                context.EndRequest += new EventHandler(this.OnLeave);
            }
        }

        private void OnEnter(object source, EventArgs eventArgs)
        {
            this._isOnEnterCalled = true;

            var httpCtx = HttpContext.Current;
            if (httpCtx.User != null)
            {
                // do nothing because someone else authenticated
                return;
            }

            //ignore css,js etc. 
            var path = httpCtx.Request.Path;
            if (!string.IsNullOrEmpty(path))
            {
                if (IgnoreExtensions.Exists(item => path.EndsWith(item)))
                {
                    return;
                }
            }

            //check for toke url parameter
            var token = httpCtx.Request.QueryString[PortalAuthenticationConfig.TokenUrlParameterName];
            if (!string.IsNullOrWhiteSpace(token))
            {
                this.AuthenticateByUrl(httpCtx, token);
            }

            //check for auth cookie
            var authCookie = httpCtx.Request.Cookies[PortalAuthenticationConfig.AuthCookieName];
            if (authCookie != null)
            {
                this.AuthenticateByCookie(httpCtx, authCookie);
                return;
            }

            //if access login page, skip Authorization
        }

        private void OnLeave(object source, EventArgs eventArgs)
        {
            if (this._isOnEnterCalled)
            {
                this._isOnEnterCalled = false;
            }
            else
            {
                return;
            }

            var app = (HttpApplication)source;
            var context = app.Context;

            //if status code not equal 401, just return 
            if (context.Response.StatusCode != 401)
                return;

            //if has WWW-Authenticate head, just return and don't redirect
            if (context.Response.Headers.AllKeys.Contains(WWWAuthenticate))
                return;

            // Don't do it if already there is ReturnUrl, already being redirected,
            // to avoid infinite redirection loop
            String strUrl = context.Request.RawUrl;
            if (strUrl.IndexOf("?" + PortalAuthenticationConfig.ReturnUrl + "=", StringComparison.Ordinal) != -1
                || strUrl.IndexOf("&" + PortalAuthenticationConfig.ReturnUrl + "=", StringComparison.Ordinal) != -1)
            {
                return;
            }

            string loginUrl = PortalAuthenticationConfig.PortalLoginUrl;
            if (loginUrl == null || loginUrl.Length <= 0)
                throw new HttpException("Invalid Login url.");

            string strRedirect;
            if (PortalAuthenticationHelper.IsAccessingPortalHomeOrLoginPage(context.Request.Url))
            {
                strRedirect = loginUrl;
            }
            else
            {
                //other application, add as return url
                strRedirect = loginUrl + "?" + PortalAuthenticationConfig.ReturnUrl + "=" + this.BuildReturnUrl(context);
            }

            context.Response.Redirect(strRedirect, false);
        }

        private void AuthenticateByCookie(HttpContext httpCtx, HttpCookie authCookie)
        {
            var token = authCookie.Value;
            if (string.IsNullOrEmpty(token))
            {
                //token was damaged, return
                return;
            }

            var userInfo = this.GetUserInfoFromPortal(token);
            if (userInfo != null)
            {                              
                httpCtx.User = PortalAuthenticationHelper.LoginedIn(userInfo);
            }

            //if it is get request and the url just include token parameter, remove it and redirect 
            var request = httpCtx.Request;
            if (request.HttpMethod.ToUpper() == "GET" && (request.QueryString.Count == 1) && (request.QueryString.Keys.Count > 0))
            {
                var key = request.QueryString.Keys[0];
                if (key != null && (key.ToLower() == PortalAuthenticationConfig.TokenUrlParameterName.ToLower()))
                {
                    var strRedirect = this.ReBuildUrlExceptToken(httpCtx);
                    httpCtx.Response.Redirect(strRedirect, true);
                }
            }
        }

        private void AuthenticateByUrl(HttpContext httpCtx, string token)
        {
            var userInfo = this.GetUserInfoFromPortal(token);
            if (userInfo != null)
            {
                httpCtx.User = PortalAuthenticationHelper.LoginedIn(userInfo);

                //remove token from url 
                var strRedirect = this.ReBuildUrlExceptToken(httpCtx);
                //redirect immediately
                httpCtx.Response.Redirect(strRedirect, true);
            }
        }

        private string ReBuildUrlExceptToken(HttpContext context)
        {
            var queryStrBuilder = new StringBuilder();
            var isShowFrame = false;//是否从frame中显示
            foreach (var key in context.Request.QueryString.AllKeys)
            {
                if (key.ToLower() == PortalAuthenticationConfig.TokenUrlParameterName.ToLower())
                {
                    continue;
                }
                if (key.ToLower() == PortalAuthenticationConfig.PortalFrameName.ToLower())
                {
                    isShowFrame = true;
                    continue;
                }
                queryStrBuilder.AppendFormat("{0}={1}", key, context.Request.QueryString[key]);
            }
            var redirectHostUrl = string.Format("{0}://{1}:{2}{3}", context.Request.Url.Scheme, context.Request.Url.Host, context.Request.Url.Port, context.Request.Url.AbsolutePath);
            if (isShowFrame)
            {
                redirectHostUrl = string.Format("/?{0}={1}", PortalAuthenticationConfig.PortalDefaultUrl, redirectHostUrl);
                if (queryStrBuilder.Length > 0)
                {
                    redirectHostUrl = string.Format("{0}&{1}", redirectHostUrl, queryStrBuilder);
                }
            }
            else if (queryStrBuilder.Length > 0)
            {
                redirectHostUrl = string.Format("{0}?{1}", redirectHostUrl, queryStrBuilder);
            }
            return redirectHostUrl;
        }

        private UserPackageInfo GetUserInfoFromPortal(string token)
        {
            var getUserResponse = UserServiceClient.GetUserByToken(token);
            if (getUserResponse != null && getUserResponse.IsValid())
            {
                return new UserPackageInfo(getUserResponse.LoginName,
                        getUserResponse.DisplayName,
                        token,
                        (UserType)Enum.Parse(typeof(UserType), getUserResponse.UserType),
                        getUserResponse.RoleCodes,
                        getUserResponse.PermissionCodes);
            }

            return null;
        }

        private string BuildReturnUrl(HttpContext context)
        {
            var returnUrl = PortalAuthenticationConfig.IsFixedRedirect
                ? PortalAuthenticationConfig.FixedRedirect
                : context.Request.Url.ToString();
            return HttpUtility.UrlEncode(returnUrl, context.Request.ContentEncoding);
        }
    }
}
