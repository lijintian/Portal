using System.Collections.Generic;
using Portal.Web.Core.Model;

namespace Portal.Web.Core
{
    public class IDropDownListSource
    {
        #region 01.递归获取下拉框的数据源
        /// <summary>
        /// 递归获取下拉框的数据源
        /// 如果orderBy＝true,先根据OrderBy再根据Name排序，否则，只根据Name排序
        /// </summary>
        /// <param name="list"></param>
        /// <param name="parentid"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        public static List<EnumModel> GetChildList(List<IDropDownListInfo> list, string parentid, bool orderBy = false, string id = "")
        {
            return ChildDropDownList<IDropDownListInfo, int>.GetChildList(list, parentid, orderBy, id);
        }
        /// <summary>
        /// 递归获取下拉框的数据源
        /// 如果orderBy＝true,先根据OrderBy再根据Name排序，否则，只根据Name排序
        /// </summary>
        public static List<IDropDownListInfo> GetChildList2(List<IDropDownListInfo> list, string parentid, bool orderBy = false, string id = "")
        {
            return ChildDropDownList<IDropDownListInfo, int>.GetChildList2(list, parentid, orderBy, id);
        }
        #endregion

        #region 02.获取同级信息
        /// <summary>
        /// 获取同级信息
        /// </summary>
        /// <param name="list"></param>
        /// <param name="levelid"></param>
        /// <returns></returns>
        public static List<EnumModel> GetSameLevelList(List<IDropDownListInfo> list, int levelid, string parentid)
        {
            return ChildDropDownList<IDropDownListInfo, int>.GetSameLevelList(list, levelid, parentid);
        }
        /// <summary>
        /// 获取同级信息
        /// </summary>
        /// <param name="list"></param>
        /// <param name="levelid"></param>
        /// <param name="parentid">最顶级Id</param>
        /// <returns></returns>
        public static List<IDropDownListInfo> GetSameLevelList2(List<IDropDownListInfo> list, int levelid, string parentid)
        {
            return ChildDropDownList<IDropDownListInfo, int>.GetSameLevelList2(list, levelid, parentid);
        }
        #endregion

        #region 03.递归获取子节点id
        /// <summary>
        /// 递归获取子节点id
        /// </summary>
        public static string GetChilderIds(List<IDropDownListInfo> list)
        {
            return ChildDropDownList<IDropDownListInfo, int>.GetChilderIds(list);
        }

        /// <summary>
        /// 递归获取子节点id
        /// </summary>
        /// <param name="list">查找集合</param>
        /// <param name="parentid">父节点ID</param>
        /// <returns></returns>
        public static string GetChilderIds(List<IDropDownListInfo> list, string parentid)
        {
            return ChildDropDownList<IDropDownListInfo, int>.GetChilderIds(list, parentid);
        }
        #endregion
    }
}
