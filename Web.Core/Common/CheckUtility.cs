using System;
using System.Text;
using System.Text.RegularExpressions;

namespace Portal.Web.Core.Common
{
    public static class CheckUtility
    {
        #region 01.判断字符是否为空
        /// <summary>
        /// 判断字符是否为空
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsEmpty(string str)
        {
            return string.IsNullOrEmpty(str);
        }
        #endregion

        #region 02.判断字符串是否为整型
        /// <summary>
        /// 判断字符串是否为整型
        /// </summary>
        /// <param name="source"></param>
        /// <param name="canEmpty">是否允许为空</param>
        /// <returns></returns>
        public static bool IsInteger(string source, bool canEmpty = false)
        {
            if (string.IsNullOrEmpty(source))
            {
                return canEmpty;
            }
            if (Regex.IsMatch(source, "^[1-9]\\d*$"))
            {
                return true;
            }
            return false;
        }
        #endregion

        #region 03.判断是否是字母数字下划线
        /// <summary>
        /// 判断是否是字母数字下划线
        /// </summary>
        /// <param name="source"></param>
        /// <param name="canEmpty">是否允许为空</param>
        /// <returns></returns>
        public static bool IsNumLetter(string source, bool canEmpty = false)
        {
            if (string.IsNullOrEmpty(source))
            {
                return canEmpty;
            }
            if (Regex.IsMatch(source, "^[A-Za-z0-9_-]*$"))
            {
                return true;
            }
            return false;
        }
        #endregion

        #region 04.判断是否是有效链接
        /// <summary>
        /// 判断是否是有效链接
        /// </summary>
        /// <param name="source"></param>
        /// <param name="canEmpty">是否允许为空</param>
        /// <returns></returns>
        public static bool IsUrl(string source, bool canEmpty = false)
        {
            if (string.IsNullOrEmpty(source))
            {
                return canEmpty;
            }
            string pattern = @"(https|http|ftp|rtsp|mms):\/\/[\w\-_]+(\.[\w\-_]+)+([\w\-\.,@?^=%&amp;:/~\+#]*[\w\-\@?^=%&amp;/~\+#])?";
            if (Regex.IsMatch(source, pattern, RegexOptions.IgnoreCase))
            {
                return true;
            }
            return false;
        }
        #endregion

        #region 05.获取字符串的长度(按byte)
        /// <summary>
        /// 获取字符串的长度(按byte)
        /// </summary>
        /// <param name="mText">字符串</param>
        /// <returns>字符串长度</returns>
        public static Int32 GetByteLen(this string mText)
        {
            return Encoding.Default.GetByteCount(mText);
        }
        #endregion
    }
}
