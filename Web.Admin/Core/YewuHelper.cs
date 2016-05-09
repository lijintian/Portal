using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Portal.Web.Admin.Core
{
    static class YewuHelper
    {
        public static void Logout()
        {
            var cookieName = AppSettingHelper.Get("YewuCookieName");
            var cookieDomain = AppSettingHelper.Get("YewuCookieDomain");
            var cookie = new HttpCookie(cookieName);
            cookie.Values.Clear();
            cookie.Value = null;
            cookie.Expires = DateTime.Now.AddMonths(-1);
            cookie.Domain = cookieDomain;
            System.Web.HttpContext.Current.Response.Cookies.Add(cookie);
        }
    }
}
