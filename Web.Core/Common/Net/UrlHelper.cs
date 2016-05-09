using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace Portal.Web.Core
{
    /// <summary>
    /// by LixingTie
    /// </summary>
    public class UrlHelper
    {
        #region 字段

        /// <summary>
        /// 首页关键字
        /// </summary>
        private static readonly string[] IndexKeys = { "^((http|https)://)?[^/]*?/?$" };

        /// <summary>
        /// 博客关键字
        /// </summary>
        private static readonly string[] BlogKeys = { "blog", "bokee" };

        /// <summary>
        /// 微博关键字
        /// </summary>
        private static readonly string[] MicroBlogKeys =
            {
                "weibo", "twritter", @"://t\.sina", @"://t\.163",
                @"://t\.sohu", @"://t\.qq", @"://t\.people", @"://t\.", "fanfou"
            };

        /// <summary>
        /// 新闻关键字
        /// </summary>
        private static readonly string[] NewsKeys =
            {
                "news", @"[^\d]\d{8}[^\d]", @"\d{4}[^\d]\d{1, 2}[^\d]{1, 2}",
                @"\d{1, 2}[^\d]\d{4}[^\d]{1, 2}"
            };

        /// <summary>
        /// 论坛关键字
        /// </summary>
        private static readonly string[] BbsKeys =
            {
                "bbs", "forum", "club", "post", "thread", "tieba", "tid", "fid",
                "board", "showtopic"
            };
        private static readonly Regex DomainRegex = RegexUtility.CreateRegex(@"(?<Domain>([^:\./ ]+\.(com\.cn|net\.cn|org\.cn|gov\.cn|com|cn|mobi|tel|asia|net|org|name|me|tv|cc|hk|biz|info|in|[a-z]{2,})|\d+\.\d+.\d+.\d+))($|\s|/)");
        #endregion

        #region 获娶相对地址
        /// <summary>
        /// 获娶相对地址
        /// </summary>
        /// <param name="baseUri"></param>
        /// <param name="childUri"></param>
        /// <returns></returns>
        public static string AbsoluteUrl(string baseUri, string childUri)
        {
            return new Uri(new Uri(baseUri), childUri).AbsoluteUri;
        }
        #endregion

        #region 获取域名
        /// <summary>
        /// 获取域名
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string GetDomainName(string url)
        {
            var matchs = DomainRegex.Matches(url);
            if (matchs.Count == 0)
            {
                return string.Empty;
            }
            return matchs[0].Groups["Domain"].Value;

        }

        #endregion

        #region 对象转为URL参数
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
    }
}
