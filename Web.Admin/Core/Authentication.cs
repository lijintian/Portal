using Portal.Web.Admin.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml;

namespace Portal.Web.Admin.Core
{
    public class Authentication
    {
        public static readonly string Login_COOKIED_Name = System.Configuration.ConfigurationManager.AppSettings["Login_COOKIED_Name"];

        /// <summary>
        /// 检查当前请求中是否包含指定Cookie
        /// </summary>    
        /// <returns></returns>
        public static bool IsLoginCookieExist()
        {
            if (System.Web.HttpContext.Current.Request.Cookies[Login_COOKIED_Name] != null)
                return true;
            else
                return false;
        }
        public static LoginResponse GetLoginToken()
        {
            LoginResponse response = new LoginResponse();
            //先检查request
            var loginCookie = GetCookie(Login_COOKIED_Name);
            if (string.IsNullOrEmpty(loginCookie))
            {
                response.ReturnCode = -1;
                response.ErrorDescription = "Token不存在";
            }
            else
            {
                string token = loginCookie;
                response.Token = token;
            }
            return response;
        }
      
        /// <summary>
        /// 得到Cookies值
        /// </summary>
        /// <param name="cookiename"></param>
        public static string GetCookie(string cookiename)
        {
            HttpCookie cookies = System.Web.HttpContext.Current.Request.Cookies[cookiename];
            if (cookies != null)
            {
                return cookies.Value;
            }
            else return "";
        }
        public static LoginResponse CreateResponse(string ReturnURL, string token)
        {
            LoginResponse ssresponse = new LoginResponse();
            ssresponse.ReturnURL = ReturnURL;
            ssresponse.ReturnCode = 1;
            ssresponse.Token = token;
            return ssresponse;
        }
        public static void SetLoginCookie(string token)
        {
            string cookiedvalue =token;

            SetCookie(Login_COOKIED_Name, cookiedvalue);
        }
        public static void RemoveLoginCookie()
        {
            if (IsLoginCookieExist())
            {
                HttpCookie cookie = System.Web.HttpContext.Current.Request.Cookies[Login_COOKIED_Name];
                cookie.Values.Clear();
                cookie.Value = null;
                cookie.Expires = DateTime.Now.AddDays(-1);
                HttpContext.Current.Response.Cookies.Add(cookie);
            }
        }
        /// <summary>
        /// 设置Cookie
        /// </summary>
        /// <param name="CookieName"></param>
        /// <param name="CookieValue"></param>       
        public static void SetCookie(string cookieName, string cookieValue)
        {
            HttpCookie cookie = new HttpCookie(cookieName);
            cookie.Value = cookieValue;
            System.Web.HttpContext.Current.Response.Cookies.Add(cookie);
        }
        

    }
}
