using System;
using System.Configuration;

namespace Portal.Web.Core
{
    public class AppSettingsUtility
    {
        #region 01.获取键值对的值
        /// <summary>
        /// <para>获取配置文件中当前键值对应的值，并转换为相应的类型</para>
        /// <para>当配置项为空，返回传入的默认值</para>
        /// </summary>
        /// <param name="key">键值</param>
        /// <returns>配置项值</returns>
        public static string Get(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }

        /// <summary>
        /// 获取webconfig的值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultstr"></param>
        /// <returns></returns>
        public static int GetInt(string key, int defaultstr)
        {
            return Get(key, defaultstr);
        }
        /// <summary>
        /// 获取webconfig的值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static Guid GetGuid(string key)
        {
            string value = Get<string>(key);
            return new Guid(value);
        }
        #endregion

        #region 02.获取配置文件中当前键值对应的值，并转换为相应的类型
        /// <summary>
        /// <para>获取配置文件中当前键值对应的值，并转换为相应的类型</para>
        /// <para>当配置项为空时，自动转换为该类型的默认值</para>
        /// </summary>
        /// <typeparam name="T">想要转换的类型</typeparam>
        /// <param name="key">键值</param>
        /// <returns>配置项值</returns>
        public static T Get<T>(string key)
        {
            return Get(key, default(T));
        }

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
