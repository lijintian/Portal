using System.Collections.Generic;
using System.Linq;
using Portal.Web.Core.Model;

namespace Portal.Web.Core
{
    public class DropDownListSource
    {
        #region 01.递归获取下拉框的数据源
        /// <summary>
        /// 递归获取下拉框的数据源
        /// 如果orderBy＝true,先根据OrderBy再根据Name排序，否则，只根据Name排序
        /// </summary>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// List<DropDownListInfo> list = new List<DropDownListInfo>
        ///    {
        ///       new DropDownListInfo("1", "中国", "0"),
        ///       new DropDownListInfo("2", "北京", "1"),
        ///       new DropDownListInfo("3", "上海", "2"),
        ///       new DropDownListInfo("4", "广州", "3"),
        ///       new DropDownListInfo("5", "武汉", "2"),
        ///       new DropDownListInfo("6", "长沙", "2")
        ///   };
        /// var temp=ChildDropDownList.GetSouceList(list,"0",true);
        /// var selectlist=ListSource.GetSource(temp);
        /// ]]>
        /// </code>
        /// </example>
        /// <param name="list"></param>
        /// <param name="parentid"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        public static List<EnumModel> GetChildList(List<DropDownListInfo> list, string parentid, bool orderBy = false, string id = "")
        {
            return ChildDropDownList<DropDownListInfo, string>.GetChildList(list, parentid, orderBy, id);
        }

        /// <summary>
        /// 递归获取下拉框的数据源
        /// 如果orderBy＝true,先根据OrderBy再根据Name排序，否则，只根据Name排序
        /// </summary>
        public static List<DropDownListInfo> GetChildList2(List<DropDownListInfo> list, string parentid, bool orderBy = false, string id = "")
        {
            return ChildDropDownList<DropDownListInfo, string>.GetChildList2(list, parentid, orderBy, id);
        }
        #endregion

        #region 02.获取同级信息
        /// <summary>
        /// 获取同级信息
        /// </summary>
        /// <param name="list"></param>
        /// <param name="levelid"></param>
        /// <returns></returns>
        public static List<EnumModel> GetSameLevelList(List<DropDownListInfo> list, int levelid, string parentid)
        {
            return ChildDropDownList<DropDownListInfo, string>.GetSameLevelList(list, levelid, parentid);
        }
        /// <summary>
        /// 获取同级信息
        /// </summary>
        /// <param name="list"></param>
        /// <param name="levelid"></param>
        /// <param name="parentid">最顶级Id</param>
        /// <returns></returns>
        public static List<DropDownListInfo> GetSameLevelList2(List<DropDownListInfo> list, int levelid, string parentid)
        {
            return ChildDropDownList<DropDownListInfo, string>.GetSameLevelList2(list, levelid, parentid);
        }
        #endregion

        #region 03.递归获取子节点id
        /// <summary>
        /// 递归获取子节点id
        /// </summary>
        public static string GetChilderIds(List<DropDownListInfo> list)
        {
            return ChildDropDownList<DropDownListInfo, string>.GetChilderIds(list);
        }

        /// <summary>
        /// 递归获取子节点id
        /// </summary>
        /// <param name="list">查找集合</param>
        /// <param name="parentid">父节点ID</param>
        /// <returns></returns>
        public static string GetChilderIds(List<DropDownListInfo> list, string parentid)
        {
            return ChildDropDownList<DropDownListInfo, string>.GetChilderIds(list, parentid);
        }
        #endregion
    }
}
