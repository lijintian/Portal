using System;
using System.Collections.Generic;
using System.Linq;
using Portal.Web.Core.Model;

namespace Portal.Web.Core
{
    public class ArrayUtility
    {
        #region 01.字符串转换为List，去除重复项
        /// <summary>
        /// 字符串转换为List，去除重复项
        /// 使用：list = ToList(str);
        /// </summary>
        /// <param name="str">当前字符串对象</param>
        /// <returns>传入前str="a,b,a";返回结果str="a,b"</returns>
        public static List<string> ToList(string str)
        {
            return ToList(str, new char[] { ',' });
        }

        /// <summary>
        /// 字符串转换为List，去除重复项
        /// 使用：list = ToList(str, new char[] {','});
        /// </summary>
        /// <param name="str">当前字符串对象</param>
        /// <param name="splits">根据</param>
        /// <returns>传入前str="a,b,a";返回结果str="a,b"</returns>
        public static List<string> ToList(string str, char[] splits)
        {
            var list = GetList(str, splits);
            return list == null ? null : list.ToList();
        }
        #endregion

        #region 02.字符串转换为数组，去除重复项
        /// <summary>
        /// 字符串转换为数组，去除重复项
        /// 使用：list = ToArray(str);
        /// </summary>
        /// <param name="str">当前字符串对象</param>
        /// <returns>传入前str="a,b,a";返回结果str="a,b"</returns>
        public static string[] ToArray(string str)
        {
            return ToArray(str, new char[] { ',' });
        }

        /// <summary>
        /// 字符串转换为List，去除重复项
        /// 使用：list = ToArray(str, new char[] {','});
        /// </summary>
        /// <param name="str">当前字符串对象</param>
        /// <param name="splits">根据</param>
        /// <returns>传入前str="a,b,a";返回结果str="a,b"</returns>
        public static string[] ToArray(string str, char[] splits)
        {
            var list = GetList(str, splits);
            return list == null ? null : list.ToArray();
        }
        #endregion

        #region 03.排除字符串内的重复项
        /// <summary>
        /// 排除字符串内的重复项
        /// <example>
        /// <code>
        /// array[1] = FilterRepeatItem(array[1]);
        /// </code>
        /// </example>
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string FilterEmpty(string str)
        {
            return FilterEmpty(str, new char[] { ',' });
        }

        /// <summary>
        /// 排除字符串内的重复项
        /// <example>
        /// <code>
        /// array[1] = FilterRepeatItem(array[1], new char[] {','});
        /// </code>
        /// </example>
        /// </summary>
        /// <param name="str">当前字符串对象</param>
        /// <param name="splits">根据</param>
        /// <returns>传入前str="a,b,a";返回结果str="a,b"</returns>
        public static string FilterEmpty(string str, char[] splits)
        {
            var list = GetList(str, splits);
            if (list == null)
            {
                return str;
            }
            str = string.Join(",", list);
            return str;
        }
        #endregion

        #region 04.返回去重去空后的结果集
        /// <summary>
        /// 返回去重去空后的结果集
        /// </summary>
        /// <param name="str">当前字符串对象</param>
        /// <param name="splits">根据</param>
        /// <returns></returns>
        private static IEnumerable<string> GetList(string str, char[] splits)
        {
            if (string.IsNullOrEmpty(str))
            {
                return null;
            }
            if (splits == null || splits.Length == 0)
            {
                return null;
            }
            return str.Split(splits, StringSplitOptions.RemoveEmptyEntries).Distinct();
        }
        #endregion

        #region 05.去除数组里面的空值
        /// <summary>
        /// 去除数组里面的空值
        /// </summary>
        /// <typeparam name="T">指定类型</typeparam>
        /// <param name="args">数组</param>
        /// <returns>去除空值后的数组</returns>
        public static T[] RemoveNull<T>(T[] args)
        {
            if (args == null || args.Length == 0)
                return new T[0];

            // 用list来保存
            var list = args.Where(t => t != null && !"".Equals(t)).ToList();
            return list.ToArray<T>();
        }
        #endregion

        #region 06.去除数组里面的重复值
        /// <summary>
        /// 去除数组里面的重复值
        /// </summary>
        /// <typeparam name="T">指定类型</typeparam>
        /// <param name="args">数组</param>
        /// <returns>去除重复值后的数组</returns>
        public static T[] RemoveDuplicate<T>(T[] args)
        {
            if (args == null || args.Length == 0)
                return new T[0];
            // 用list来保存
            var list = new List<T>();
            foreach (T t in args)
            {
                // 保证没有重复
                if (!list.Contains(t))
                {
                    list.Add(t);
                }
            }
            return list.ToArray<T>();
        }
        #endregion

        #region 07.去除数组里面的空值和重复值
        /// <summary>
        /// 去除数组里面的空值和重复值
        /// </summary>
        /// <typeparam name="T">指定类型</typeparam>
        /// <param name="args">数组</param>
        /// <returns>去除空值和重复值后的数组</returns>
        public static T[] RemoveNullAndDuplicate<T>(T[] args)
        {
            if (args == null || args.Length == 0)
                return new T[0];

            // 用list来保存
            var list = new List<T>();
            foreach (T t in args)
            {
                // 保证没有重复
                if (t != null && !"".Equals(t) && !list.Contains(t))
                {
                    list.Add(t);
                }
            }
            return list.ToArray<T>();
        }
        #endregion

        #region 08.克隆List
        public static List<T> Clone<T>(List<T> list)
        {
            if (LengthUtility.IsNullOrEmpty(list)) return null;
            return new List<T>(list.ToArray());
        }
        public static T[] Clone<T>(T[] list)
        {
            if (LengthUtility.IsNullOrEmpty(list)) return null;
            T[] array2 = new T[list.Length];
            Array.Copy(list, 0, array2, 0, list.Length);
            return array2;
        }
        #endregion

        #region 09.分页检索
        /// <summary>
        /// 获取分页结果
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <param name="list"></param>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public static List<T1> GetList<T1>(List<T1> list, IBaseParameter parameter)
        {
            if (parameter.NoPage || LengthUtility.IsNullOrEmpty(list))
            {
                return list;
            }
            parameter.TotalRows = list.Count();
            return list.Skip((parameter.PageIndex - 1) * parameter.PageSize).Take(parameter.PageSize).ToList();
        }
        /// <summary>
        /// 获取分页结果
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <param name="list"></param>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public static List<T1> GetList<T1>(IEnumerable<T1> list, IBaseParameter parameter)
        {
            if (parameter.NoPage)
            {
                return list.ToList();
            }
            parameter.TotalRows = list.Count();
            return list.Skip((parameter.PageIndex - 1) * parameter.PageSize).Take(parameter.PageSize).ToList();
        }
        #endregion
    }
}
