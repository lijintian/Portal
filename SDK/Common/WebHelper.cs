using System;
using System.Configuration;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using Portal.SDK.Security;
using ServiceStack.Text;

namespace Portal.SDK.Common
{
    public class WebHelper
    {
        #region 获取应用系统ID
        public static string GetAppName()
        {
            string appId = string.Empty;
            try
            {
                appId = CK1PortalAuthenticationConfig.ApplicationName;
            }
            catch (Exception)
            {
            }
            if (string.IsNullOrEmpty(appId))
            {
                appId = Environment.MachineName;
            }
            return appId;
        }
        #endregion

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

        #region 04.对象转为URL参数
        public static string GetUrl(string url, object parameter)
        {
            var data = WebHelper.ObjectToUrlParameter(parameter);
            url = string.Format("{0}{1}{2}", url, url.IndexOf("?") <= 0 ? "?" : "&", data);
            return url;
        }

        public static string GetUrl(string url, string param, string value)
        {
            url = string.Format("{0}{1}{2}={3}", url, url.IndexOf("?") <= 0 ? "?" : "&", param, HttpUtility.UrlEncode(value));
            return url;
        }
        /// <summary>
        /// 对象转为URL参数
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ObjectToUrlParameter(object obj)
        {
            if (obj == null) return string.Empty;
            var properties = obj.GetType().GetProperties();
            var psb = new StringBuilder();
            string v;
            foreach (var p in properties)
            {
                var value = p.GetValue(obj, null);
                v = value != null ? value.ToString() : string.Empty;
                var type = p.PropertyType.FullName;
                var index = type.IndexOf("[");
                // 当值为复杂类型时(判断依据：v返回的值跟type值类似，都是表示类路径的)
                if (value != null && index > 0 && v.EndsWith("]") && type.EndsWith("]") &&
                    v.Length > index && v.Substring(0, index).Equals(type.Substring(0, index)))
                {
                    v = value.ToJson();
                    // 如果是数组，去掉数组的前后中括号
                    if (v.StartsWith("[") && v.EndsWith("]"))
                    {
                        v = v.Substring(1);
                        v = v.Substring(0, v.Length - 1);
                    }
                    // 如果里面包含的是字符串，去掉括号(字符串里面包含逗号时会引起歧义)
                    if (v.StartsWith("\"") && v.EndsWith("\""))
                    {
                        v = v.Trim('"').Replace("\",\"", ",");
                    }
                }
                v = HttpUtility.UrlEncode(v);
                psb.AppendFormat("{0}={1}&", new object[] { p.Name, v });
            }
            return psb.ToString().TrimEnd(new[] { '&' });
        }
        #endregion

        #region 05.获取键值对
        /// <summary>
        /// <para>获取配置文件中当前键值对应的值，并转换为相应的类型</para>
        /// <para>当配置项为空，返回传入的默认值</para>
        /// </summary>
        /// <typeparam name="T">想要转换的类型</typeparam>
        /// <param name="key">键值</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>配置项值</returns>
        public static T Get<T>(string key, T defaultValue)
        {
            var v = ConfigurationManager.AppSettings[key];
            return String.IsNullOrEmpty(v) ? defaultValue : (T)Convert.ChangeType(v, typeof(T));
        }
        #endregion
    }
}
