/*
 EasyDDD
系统名称：  字符串处理类
模块名称：  正则表达式操作类
创建日期：  2015-08-06

内容摘要：  
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace Portal.Web.Core
{
    public static class RegexUtility
    {
        #region 字段
        /// <summary>
        /// 正则表达式缓存
        /// </summary>
        private static readonly Dictionary<string, Regex> RegexCache = new Dictionary<string, Regex>();

        /// <summary>
        /// 正则表达式选项：编译，忽略大小写，忽略无命名组。
        /// </summary>
        public static RegexOptions CompiledIgnoreCaseExplicitCapture = RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture;
        #endregion

        #region 创建正规表达式
        /// <summary>
        /// 构建编译，忽略大小写，忽略无组名的正则表达式，如果已经存在则直接返回缓存的
        /// </summary>
        /// <param name="pattern"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public static Regex CreateRegex(string pattern)
        {
            Regex reg;
            if (!RegexCache.TryGetValue(pattern, out reg))
            {
                reg = new Regex(pattern, CompiledIgnoreCaseExplicitCapture);
                RegexCache.Add(pattern, reg);
            }
            return reg;
        }
        #endregion

        #region 03.获取第一个组名里的第一个匹配项
        /// <summary>
        /// 获取第一个组名里的第一个匹配项
        /// </summary>
        /// <param name="reg"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string GetFirstMatch(this Regex reg, string content)
        {
            var mc = reg.Matches(content);
            if (mc.Count == 0) return string.Empty;

            var groupName = GetFirstGroupName(reg);
            return String.IsNullOrEmpty(groupName) ? mc[0].Value : mc[0].Groups[groupName].Value.Trim();
        }
        #endregion

        #region 04.获取匹配到的第一个组
        /// <summary>
        /// 获取匹配到的第一个组
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="reg"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public static T GetFirstMatch<T>(this Regex reg, string content)
        {
            content = GetFirstMatch(reg, content);
            return String.IsNullOrEmpty(content) ? default(T) : (T)Convert.ChangeType(content, typeof(T));
        }
        #endregion

        #region 05.获取第一个组名
        /// <summary>
        /// 获取第一个组名
        /// </summary>
        /// <param name="reg"></param>
        /// <returns></returns>
        public static string GetFirstGroupName(this Regex reg)
        {
            return reg.GetGroupNames().Length >= 2 ? reg.GetGroupNames()[1] : string.Empty;
        }
        #endregion

        #region 06.获取匹配的结果集
        /// <summary>
        /// 获取匹配的结果集
        /// </summary>
        /// <param name="content"></param>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public static List<string> GetMatchList(string content, string pattern)
        {
            return Regex.Matches(content, pattern).Cast<Match>().Select(a => a.Value).ToList();
        }
        /// <summary>
        /// 获取中括号里面和外面的字符
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static List<string> GetMatchListByBracket(string content)
        {
            //1.获取中括号里面和外面的字符：GetMatchList("111[222]3333[444]555[666]aaa[asf]", @"(?<=([[\[\]]+)?)[^]\[\]]+");
            //2.获取中文中括号里面和外面的字符：GetMatchList("111[222】3333【444]555【666】aaa[asf]",@"(?<=([[【】]+)?)[^]【】]+");
            //3.获取中英文中括号里面和外面的字符：GetMatchList("111[222】3333【444]555【666】aaa[asf]", @"(?<=([[(\[|【)(\]|】)]+)?)[^](\[|【)(\]|】)]+");
            return GetMatchList(content, @"(?<=([[\[\]]+)?)[^]\[\]]+");
        }
        /// <summary>
        /// 获取中文中括号里面和外面的字符
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static List<string> GetMatchListByBracketCh(string content)
        {
            return GetMatchList(content, @"(?<=([[【】]+)?)[^]【】]+");
        }
        #endregion

        #region  正则替换字符串
        /// <summary>
        /// 正则替换字符串
        /// </summary>
        /// <param name="strSource">源字符串</param>
        /// <param name="strPattern">正则表达式</param>
        /// <param name="strReplace">要替换成的字符</param>
        /// <returns></returns>
        public static string RegexReplace(string strSource, string strPattern, string strReplace)
        {
            return RegexReplace(strSource, strPattern, strReplace, true);
        }
        #endregion

        #region 正则替换字符串
        /// <summary>
        /// 正则替换字符串
        /// </summary>
        /// <param name="strSource">源字符串</param>
        /// <param name="strPattern">正则表达式</param>
        /// <param name="strReplace">要替换成的字符</param>
        /// <param name="ignoreCase">是否忽略大小写</param>
        /// <returns></returns>
        public static string RegexReplace(string strSource, string strPattern, string strReplace, bool ignoreCase)
        {
            return Regex.Replace(strSource, strPattern, strReplace, ignoreCase ? RegexOptions.IgnoreCase : RegexOptions.None);
        }
        #endregion
    }
}
