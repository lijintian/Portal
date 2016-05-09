/*
 EasyDDD
系统名称：  基础类库
模块名称：  JsonUtility
创建日期：  2007-2-1

内容摘要： 
*/
using System.Net;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;

namespace Portal.Web.Core
{
    /// <summary>
    /// JSON 扩展，支持FromUrl
    /// </summary>
    public static class JsonUtility
    {
        #region 属性
        public static CookieContainer Credence { get; set; }
        #endregion

        #region 转换成 JSON
        /// <summary>
        /// 转换成 JSON
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static string ToJson(this object o)
        {
            var serializer = new JavaScriptSerializer();
            return serializer.Serialize(o);
        }

        /// <summary>
        /// 转换成 JSON
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static string ToUrlEncodeJson(this object o)
        {
            var serializer = new JavaScriptSerializer();
            var jason = serializer.Serialize(o);
            return HttpUtility.UrlEncode(jason, Encoding.UTF8);
        }
        #endregion

        #region 将 JSON 转换为对象
        /// <summary>
        /// 将 JSON 转换为对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="s"></param>
        /// <returns></returns>
        public static T FromJson<T>(this string s)
        {
            var serializer = new JavaScriptSerializer();
            return serializer.Deserialize<T>(s);
        }

        /// <summary>
        /// 将 JSON 转换为对象
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static object FromJson(this string s)
        {
            var serializer = new JavaScriptSerializer();
            return serializer.DeserializeObject(s);
        }

        public static T FromJson<T>(this string s, int? recursionLimit)
        {
            if (string.IsNullOrEmpty(s))
            {
                return default(T);
            }
            var serializer = new JavaScriptSerializer();
            serializer.RecursionLimit = recursionLimit ?? serializer.RecursionLimit;
            return serializer.Deserialize<T>(s);
        }

        public static T FromJson<T>(this string s, int? recursionLimit, int? maxJsonLengthMultiple)
        {
            if (string.IsNullOrEmpty(s))
            {
                return default(T);
            }
            var serializer = new JavaScriptSerializer();
            serializer.RecursionLimit = recursionLimit ?? serializer.RecursionLimit;
            if (maxJsonLengthMultiple != null && maxJsonLengthMultiple.Value > 0)
            {
                serializer.MaxJsonLength = serializer.MaxJsonLength * maxJsonLengthMultiple.Value;
            }
            return serializer.Deserialize<T>(s);
        }
        #endregion
    }
}
