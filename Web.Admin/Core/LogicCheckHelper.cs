using System;
using System.Web;

namespace Portal.Web.Admin.Core
{
    public class LogicCheckHelper
    {
        private static readonly string loginFailKey = "portalLoginFail";
        /// <summary>
        /// 登陆失败超过3次，出现验证码
        /// </summary>
        /// <returns></returns>
        public static bool IsOutLoginFail()
        {
            int count = GetLoginFailCount();
            return count > 1;
        }

        private static int GetLoginFailCount()
        {
            int loginFailCount = 0;
            var loginFail = HttpContext.Current.Request.Cookies[loginFailKey];
            if (loginFail != null && loginFail.Value != null)
            {
                int.TryParse(loginFail.Value, out loginFailCount);
            }
            return loginFailCount;
        }

        public static void MarkLoginFailPlus()
        {
            var loginFailCount = GetLoginFailCount();
            var cookie = new HttpCookie(loginFailKey);
            cookie.Value = (loginFailCount + 1).ToString();
            cookie.Expires = DateTime.Now.AddHours(6);
            HttpContext.Current.Response.Cookies.Add(cookie);
            HttpContext.Current.Request.Cookies.Add(cookie);
        }

        public static void ClearLoginFailCookie()
        {
            var cookie = new HttpCookie(loginFailKey);
            cookie.Expires = DateTime.MinValue;
            HttpContext.Current.Response.Cookies.Add(cookie);
            HttpContext.Current.Request.Cookies.Add(cookie);
        }
    }
}
