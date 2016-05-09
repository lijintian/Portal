using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Portal.Web.Core.Extends
{
    /// <summary>
    /// 表示异常信息
    /// </summary>
    public sealed class Error
    {
        public Error(HttpContextBase httpContext)
        {
            this.User = httpContext.User.Identity.Name;
            this.RequestUrl = httpContext.Request.Url.ToString();
            this.UrlReferrer = httpContext.Request.UrlReferrer == null ? string.Empty : httpContext.Request.Url.ToString();
            this.StatusCode = httpContext.Response.StatusCode;
            this.Forms = new NameValueCollection(httpContext.Request.Form);
            this.QueryString = new NameValueCollection(httpContext.Request.QueryString);
            this.ServerVariables = new NameValueCollection(httpContext.Request.ServerVariables);
            this.SetCookie(httpContext);
        }

        public string User
        {
            get;
            private set;
        }

        public string RequestUrl
        {
            get;
            private set;
        }

        public string UrlReferrer
        {
            get;
            private set;
        }

        public int StatusCode
        {
            get;
            private set;
        }

        public NameValueCollection Cookies
        {
            get;
            private set;
        }

        public NameValueCollection Forms
        {
            get;
            private set;
        }

        public NameValueCollection QueryString
        {
            get;
            private set;
        }

        public NameValueCollection ServerVariables
        {
            get;
            private set;
        }

        private void SetCookie(HttpContextBase context)
        {
            var requestCookies = context.Request.Cookies;
            this.Cookies = new NameValueCollection(requestCookies.Count);
            foreach (var k in requestCookies.AllKeys)
            {
                var cookie = requestCookies[k];
                this.Cookies.Add(k, string.Join(",", cookie.Values));
            }
        }
    }
}
