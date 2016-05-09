using System.Text.RegularExpressions;
using System.Web;

namespace Portal.Applications.Helper
{
    public class WebHelper
    {
        #region 01.获取用户登录IP
        /// <summary>
        /// 获取用户登录IP
        /// </summary>
        /// <returns></returns>
        public static string GetUserIP()
        {
            string userIp = string.Empty;
            try
            {
                userIp = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                //可能有代理
                if (!string.IsNullOrEmpty(userIp))
                {
                    //没有“.”肯定是非IPv4格式
                    if (userIp.IndexOf(".") == -1)
                    {
                        userIp = null;
                    }
                    else
                    {
                        if (userIp.IndexOf(",") != -1)
                        {
                            //有“,”，估计多个代理。取第一个不是内网的IP。 
                            userIp = userIp.Replace(" ", "").Replace("'", "");
                            string[] temparyip = userIp.Split(",;".ToCharArray());
                            foreach (string t in temparyip)
                            {
                                //找到不是内网的地址
                                if (IsIPAddress(t)
                                    && t.Substring(0, 3) != "10."
                                    && t.Substring(0, 7) != "192.168"
                                    && t.Substring(0, 7) != "172.16.")
                                {
                                    return t;
                                }
                            }
                        }
                        //代理即是IP格式
                        else if (IsIPAddress(userIp))
                            return userIp;
                        //代理中的内容 非IP，取IP
                        else
                            userIp = null;
                    }
                }

                if (string.IsNullOrEmpty(userIp))
                    userIp = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];

                if (string.IsNullOrEmpty(userIp))
                    userIp = HttpContext.Current.Request.UserHostAddress;
            }
            catch { }
            return (IsIPAddress(userIp) ? userIp : string.Empty);
        }
        #endregion

        #region 02.判断是否是IP地址格式 0.0.0.0
        /// <summary>
        /// 判断是否是IP地址格式 0.0.0.0
        /// </summary>
        /// <param name="str1">待判断的IP地址</param>
        /// <returns>true or false</returns>
        public static bool IsIPAddress(string str1)
        {
            if (str1 == null || str1 == string.Empty || str1.Length < 7 || str1.Length > 15) return false;

            string regformat = @"^\d{1,3}[\.]\d{1,3}[\.]\d{1,3}[\.]\d{1,3}$";

            Regex regex = new Regex(regformat, RegexOptions.IgnoreCase);
            return regex.IsMatch(str1);
        }
        #endregion

        #region 03.根据User Agent获取操作系统名称
        /// <summary>
        /// 根据User Agent获取操作系统名称
        /// </summary>
        public static string GetOsName(string userAgent)
        {
            string osVersion = "Unknown";
            if (string.IsNullOrEmpty(userAgent))
            {
                return osVersion;
            }
            if (userAgent.Contains("Windows NT 6.1"))
            {
                osVersion = "Windows 7";
            }
            else if (userAgent.Contains("Windows NT 6.0"))
            {
                osVersion = "Windows Vista/Server 2008";
            }
            else if (userAgent.Contains("Windows NT 5.2"))
            {
                osVersion = "Windows Server 2003";
            }
            else if (userAgent.Contains("Windows NT 5.1"))
            {
                osVersion = "Windows XP";
            }
            else if (userAgent.Contains("Windows NT 5"))
            {
                osVersion = "Windows 2000";
            }
            else if (userAgent.Contains("Windows NT 4"))
            {
                osVersion = "Windows NT4";
            }
            else if (userAgent.Contains("Windows Me"))
            {
                osVersion = "Windows Me";
            }
            else if (userAgent.Contains("Windows 98"))
            {
                osVersion = "Windows 98";
            }
            else if (userAgent.Contains("Windows 95"))
            {
                osVersion = "Windows 95";
            }
            else if (userAgent.Contains("Windows"))
            {
                osVersion = "Windows";
            }
            else if (userAgent.Contains("Mac"))
            {
                osVersion = "Mac";
            }
            else if (userAgent.Contains("Unix"))
            {
                osVersion = "UNIX";
            }
            else if (userAgent.Contains("Linux"))
            {
                osVersion = "Linux";
            }
            else if (userAgent.Contains("SunOS"))
            {
                osVersion = "SunOS";
            }
            return osVersion;
        }
        #endregion
    }
}
