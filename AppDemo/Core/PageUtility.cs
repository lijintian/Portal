using System;
using System.Configuration;

namespace AppDemo.Core
{
    public class PageUtility
    {
        /// <summary>
        /// 注销URL
        /// </summary>
        public static string AppDemoUrl
        {
            get
            {
                return Get("AppDemoBaseAddress", "http://appdemo.test-ABC.cn");
            }
        }

        public static T Get<T>(string key, T defaultValue)
        {
            var v = ConfigurationManager.AppSettings[key];
            return String.IsNullOrEmpty(v) ? defaultValue : (T)Convert.ChangeType(v, typeof(T));
        }
    }
}