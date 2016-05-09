using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Portal.Web.Core.Model;

namespace Portal.Web.Core
{
    public class ChildDropDownList<T, Tx>
        where T : DropDownListinfo<Tx>, new()
    {
        #region 01.递归获取下拉框的数据源
        /// <summary>
        /// 递归获取下拉框的数据源
        /// 如果orderBy＝true,先根据OrderBy再根据Name排序，否则，只根据Name排序
        /// </summary>
        public static List<EnumModel> GetChildList(List<T> list, string parentid, bool orderBy = false, string id = "")
        {
            List<T> orderList = GetChildList2(list, parentid, orderBy, id);
            if (orderList.IsNullOrEmpty())
            {
                return null;
            }
            return orderList.Select(item => new EnumModel(item.Id, item.LevelId <= 1 ? item.Name : string.Format("|{0}{1}", new string('-', (item.LevelId - 1) * 2), item.Name), item.Description, item.LevelId)).ToList();
        }
        /// <summary>
        /// 递归获取下拉框的数据源
        /// 如果orderBy＝true,先根据OrderBy再根据Name排序，否则，只根据Name排序
        /// </summary>
        public static List<T> GetChildList2(List<T> list, string parentid, bool orderBy = false, string id = "")
        {
            List<T> result = new List<T>();
            GetChildList(parentid, list, result, 1, id, orderBy);
            return result;
        }
        /// <summary>
        /// 根据父节点ID递归树方法
        /// </summary>
        /// <param name="parentid"></param>
        /// <param name="list"></param>
        /// <param name="result"></param>
        /// <param name="levelId">等级ID，从1开始</param>
        /// <param name="orderBy"></param>
        private static void GetChildList(string parentid, IEnumerable<T> list, List<T> result, int levelId, string id, bool orderBy)
        {
            T info = new T();
            var qu = string.IsNullOrEmpty(parentid) ? list.Where(u => string.IsNullOrEmpty(u.ParentId)) : list.Where(u => u.ParentId == parentid);
            if (!string.IsNullOrEmpty(id))
            {
                qu = list.Where(u => u.ParentId == parentid && u.Id != id);
            }
            List<T> parentlist = orderBy ? qu.OrderBy(u => u.OrderBy).ThenBy(u => u.Name).ToList() : qu.OrderBy(u => u.Name).ToList();
            foreach (T item in parentlist)
            {
                info = item.Clone() as T;
                info.LevelId = levelId;
                if (result.Any(u => u.Id == item.Id))
                {
                    break;
                }
                result.Add(info);
                GetChildList(item.Id, list, result, levelId + 1, id, orderBy);
            }
        }
        #endregion

        #region 02.获取同级信息
        /// <summary>
        /// 获取同级信息
        /// </summary>
        /// <param name="list"></param>
        /// <param name="levelid"></param>
        /// <param name="parentid">最顶级Id</param>
        /// <returns></returns>
        public static List<EnumModel> GetSameLevelList(List<T> list, int levelid, string parentid)
        {
            var orderList = GetSameLevelList2(list, levelid, parentid);
            return orderList.Select(item => new EnumModel(item.Id, item.Name, item.Description, item.LevelId)).ToList();
        }

        /// <summary>
        /// 获取同级信息
        /// </summary>
        /// <param name="list"></param>
        /// <param name="levelid"></param>
        /// <param name="parentid">最顶级Id</param>
        /// <returns></returns>
        public static List<T> GetSameLevelList2(List<T> list, int levelid, string parentid)
        {
            var orderList = GetChildList2(list, parentid, false);
            if (LengthUtility.IsNullOrEmpty(list) || levelid <= 0)
            {
                return null;
            }
            return orderList.Where(u => u.LevelId == levelid).ToList();
        }
        #endregion

        #region 03.递归获取子节点id
        /// <summary>
        /// 递归获取子节点id
        /// </summary>
        public static string GetChilderIds(List<T> list)
        {
            return GetChilderIds(list, Guid.Empty.ToString());
        }

        /// <summary>
        /// 递归获取子节点id
        /// </summary>
        /// <param name="list">查找集合</param>
        /// <param name="parentid">父节点ID</param>
        /// <returns></returns>
        public static string GetChilderIds(List<T> list, string parentid)
        {
            StringBuilder result = new StringBuilder();
            GetListByParentID(parentid, list, result);
            if (result.Length > 0)
            {
                result.Remove(result.Length - 1, 1);
            }
            return result.ToString();
        }

        /// <summary>
        /// 根据父节点ID递归树方法
        /// </summary>
        /// <param name="parentid"></param>
        /// <param name="list"></param>
        /// <param name="builder"></param>
        private static void GetListByParentID(string parentid, List<T> list, StringBuilder builder)
        {
            List<T> parentlist = list.Where(u => string.Equals(u.ParentId, parentid, StringComparison.CurrentCultureIgnoreCase)).ToList();
            foreach (T item in parentlist)
            {
                builder.AppendFormat("{0},", item.Id);
                GetListByParentID(item.Id, list, builder);
            }
        }
        #endregion
    }
}
