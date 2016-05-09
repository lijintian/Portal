using System;
using System.Collections.Generic;
using System.Reflection;

namespace Portal.Web.Api.Core
{
    public static class ObjectUtility
    {
        #region 字段
        private static readonly Dictionary<Type, PropertyInfo[]> AnonymousCache = new Dictionary<Type, PropertyInfo[]>();
        #endregion

        #region  01.获取对象属性值
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

            if (!AnonymousCache.TryGetValue(type, out properties))
            {
                properties = type.GetProperties();
                AnonymousCache.Add(type, properties);
            }

            return properties;
        }
        #endregion
    }
}
