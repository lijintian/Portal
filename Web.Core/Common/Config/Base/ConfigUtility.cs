using System;
using System.Reflection;

namespace Portal.Web.Core
{
    internal class ConfigUtility<T> where T : new()
    {
        #region 01.字段
        private T _config = new T();
        private static readonly ConfigUtility<T> Dal = new ConfigUtility<T>();
        #endregion

        #region 02.属性
        public static ConfigUtility<T> GetInstance
        {
            get { return Dal; }
        }
        /// <summary>
        /// 获取Config
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T Config
        {
            get
            {
                return _config;
            }
        }
        #endregion

        #region 03.初始化
        public ConfigUtility()
        {
            InitConfig();
        }
        #endregion

        #region 04.方法
        public void InitConfig()
        {
            _config = new T();
            var properties = Property.GetProperties(typeof(T));
            Type stringType = typeof(string);
            Type dataType;
            foreach (PropertyInfo property in properties)
            {
                var str = AppSettingsUtility.Get(property.Name);
                if (!string.IsNullOrEmpty(str))
                {
                    dataType = property.PropertyType;
                    Property.SetValue(_config, property, dataType == stringType ? str : Convert.ChangeType(str, dataType));
                }
            }
        }
        #endregion
    }
}
