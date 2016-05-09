using System;
using System.Threading;
using System.Web;

namespace Portal.Web.Api.Core
{
    public class RequestHelper
    {
        #region 01.根据参数名获取参数值
        /// <summary>
        /// 根据参数名获取参数值
        /// </summary>
        /// <param name="key">参数名</param>
        /// <returns></returns>
        public static string Query(string key)
        {
            HttpRequest request = HttpContext.Current.Request;
            string value = null;
            if (!string.IsNullOrEmpty(key))
            {
                value = request.QueryString[key];
                if (string.IsNullOrEmpty(value))
                {
                    value = request.Form[key];
                }
            }
            return value;
        }
        #endregion

        #region 02.获取Request参数并赋值到对象里面
        /// <summary>
        /// 获取Request参数并赋值到对象里面
        /// </summary>
        /// <typeparam name="T">对象的类型</typeparam>
        public static T GetObject<T>() where T : new()
        {
            return GetObject(new T());
        }

        /// <summary>
        /// 获取Request参数并赋值到对象里面
        /// </summary>
        /// <typeparam name="T">对象的类型</typeparam>
        public static T GetObject<T>(T obj) where T : new()
        {
            if (HttpContext.Current == null) throw new Exception("非http请求不可调用此方法");
            if (obj == null) obj = new T(); // 为空时必须接收参数,否则设的值无法传回
            // HTTP服务器请求
            HttpRequest request = HttpContext.Current.Request;
            var atts = obj.GetProperties();
            foreach (var att in atts)
            {
                if (!att.CanWrite) continue;
                string name = att.Name;
                var type = att.PropertyType;
                // 获取请求里的值
                string value = request.Form[name];
                if (value == null)
                {
                    value = request.QueryString[name];
                    if (value == null)
                    {
                        continue; // 取不到时,不用执行下面的赋值
                    }
                }
                // 能取到值, 则赋值
                try
                {
                    att.SetValue(obj, ChangeType(value.Trim(), type), null);
                }
                catch
                {
                }
            }
            return obj;
        }
        #endregion

        #region 02.强制类型转换
        public static object ChangeType<T>(object obj)
        {
            return ChangeType(obj, typeof(T));
        }
        public static object ChangeType(object obj, Type conversionType)
        {
            return ChangeType(obj, conversionType, Thread.CurrentThread.CurrentCulture);
        }
        public static object ChangeType(object obj, Type conversionType, IFormatProvider provider)
        {
            #region Nullable
            Type nullableType = Nullable.GetUnderlyingType(conversionType);
            if (nullableType != null)
            {
                if (obj == null)
                {
                    return null;
                }
                return Convert.ChangeType(obj, nullableType, provider);
            }
            #endregion
            if (typeof(System.Enum).IsAssignableFrom(conversionType))
            {
                return Enum.Parse(conversionType, obj.ToString());
            }
            return Convert.ChangeType(obj, conversionType, provider);
        }
        #endregion
    }
}
