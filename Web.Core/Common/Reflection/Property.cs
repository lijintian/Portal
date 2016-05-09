using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Portal.Web.Core
{
    public static class Property
    {
        #region 字段
        private static Dictionary<Type, PropertyInfo[]> propertyCache;
        #endregion

        #region 02.初始化
        static Property()
        {
            propertyCache = new Dictionary<Type, PropertyInfo[]>();
        }
        #endregion

        #region 根据指定类型获取属性
        /// <summary>
        /// 根据指定类型获取属性
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static PropertyInfo[] GetProperties(Type type)
        {
            if (!propertyCache.ContainsKey(type))
            {
                propertyCache[type] = type.GetProperties();
            }
            return propertyCache[type];
        }
        #endregion

        #region  获取对象属性值
        /// <summary>
        /// 获取对象属性值
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static PropertyInfo[] GetProperties(this object value)
        {
            var type = value.GetType();
            return type.GetPropertiesCached();
        }
        /// <summary>
        /// 获取对象属性集合
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static PropertyInfo[] GetPropertiesCached(this Type type)
        {
            PropertyInfo[] properties;
            if (!propertyCache.TryGetValue(type, out properties))
            {
                properties = type.GetProperties();
                propertyCache.Add(type, properties);
            }
            return properties;
        }
        #endregion

        #region 设置属性值
        public static void SetValue(object obj, PropertyInfo prop, object value)
        {
            prop.SetValue(obj, value, null);
        }
        #endregion

        #region 根据属性名称获取值
        public static object GetValue(object obj, PropertyInfo prop)
        {
            return prop.GetValue(obj, null);
        }
        /// <summary>
        /// 根据属性获取值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="prop"></param>
        /// <returns></returns>
        public static T GetValue<T>(object obj, PropertyInfo prop)
        {
            return (T)GetValue(obj, prop);

        }
        /// <summary>
        /// 根据属性名称获取值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static T GetValue<T>(object obj, string propertyName)
        {
            T value = default(T);
            var property = GetProperties(obj.GetType()).FirstOrDefault(p => p.Name == propertyName);
            if (property != null)
            {
                value = GetValue<T>(obj, property);
            }
            return value;
        }
        /// <summary>
        /// 根据属性名称获取值(静态变量的值也可以获取到)
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static string GetValue(object obj, string propertyName)
        {
            Type t = obj.GetType();
            FieldInfo fi = t.GetField(propertyName);
            return fi.GetValue(obj) as string;
        }
        #endregion
    }
}
