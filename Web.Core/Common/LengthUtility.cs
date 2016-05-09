using System;
using System.Collections.Generic;
using System.Linq;

namespace Portal.Web.Core
{
    public static class LengthUtility
    {
        #region 01.判断List是否为空
        /// <summary>
        /// 判断List是否为空
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> list)
        {
            return list == null || !list.Any();
        }
        /// <summary>
        /// 判断List是否为空
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty<T>(this List<T> list)
        {
            return list == null || list.Count <= 0;
        }
        /// <summary>
        /// 判断List是否为空
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty<T>(this T[] list)
        {
            return list == null || list.Length <= 0;
        }
        /// <summary>
        /// 判断字典是否为空
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty<T1, T2>(this Dictionary<T1, T2> list)
        {
            return list == null || list.Count <= 0;
        }
        #endregion

        #region 02.修改各种类型数组的长度

        #region 201.修改string数组的长度
        /// <summary>
        /// 修改string数组的长度
        /// </summary>
        /// <param name="myArr"></param>
        /// <param name="desiredSize"></param>
        public static void Redim(ref String[] myArr, int desiredSize)
        {
            Array.Resize(ref myArr, desiredSize);
        }
        #endregion

        #region 202.修改INT数组的长度
        /// <summary>
        /// 修改INT数组的长度
        /// </summary>
        /// <param name="myArr"></param>
        /// <param name="desiredSize"></param>
        public static void Redim(ref int[] myArr, int desiredSize)
        {
            Array.Resize(ref myArr, desiredSize);
        }
        #endregion

        #region 203.修改T数组的长度
        /// <summary>
        /// 修改T数组的长度
        /// </summary>
        /// <param name="arr"></param>
        /// <param name="newSize"></param>
        /// <returns></returns>
        public static void Redim<T>(ref T[] arr, int newSize)
        {
            Array.Resize(ref arr, newSize);
        }
        #endregion

        #endregion

        #region 03.从数组1从复制数据追加到数组2中

        #region 301.修改BYTE数组的长度
        /// <summary>
        /// 修改BYTE数组的长度
        /// </summary>
        /// <param name="arr"></param>
        /// <param name="newSize"></param>
        /// <returns></returns>
        public static byte[] Redim(byte[] arr, int newSize)
        {
            return Redim(arr, newSize);
        }
        #endregion

        #region 302.修改T数组的长度
        /// <summary>
        /// 修改T数组的长度
        /// </summary>
        /// <param name="arr"></param>
        /// <param name="newSize"></param>
        /// <returns></returns>
        public static T[] Redim<T>(T[] arr, int newSize)
        {
            if (newSize == arr.Length) return arr;
            var newArr = new T[newSize];
            Array.Copy(arr, 0, newArr, 0, Math.Min(arr.Length, newSize));
            return newArr;
        }
        #endregion

        #region 303.修改List数组的长度
        /// <summary>
        /// 修改List数组的长度
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="arr"></param>
        /// <param name="newSize"></param>
        /// <returns></returns>
        private static List<T>[] Redim<T>(List<T>[] arr, int newSize)
        {
            if (newSize == arr.Length) return arr;
            var newArr = new List<T>[newSize];
            Array.Copy(arr, 0, newArr, 0, Math.Min(arr.Length, newSize));
            return newArr;
        }
        #endregion

        #region 304.修改Array的长度
        /// <summary>
        /// 修改Array的长度
        /// </summary>
        /// <param name="origArray"></param>
        /// <param name="desiredSize"></param>
        /// <returns></returns>
        public static Array Redim(Array origArray, int desiredSize)
        {
            Type t = origArray.GetType().GetElementType();
            Array newArray = Array.CreateInstance(t, desiredSize);
            Array.Copy(origArray, 0, newArray, 0, Math.Min(origArray.Length, desiredSize));
            return newArray;
        }
        #endregion

        #endregion
    }
}
