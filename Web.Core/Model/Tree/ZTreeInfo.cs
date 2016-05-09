/*
 EasyDDD
系统名称：  Model
模块名称：  树的数据类
创建日期：  2012-2-1

内容摘要： 此类为格式化成用户树所需的内容: "{ id: " + group.ID + ", pId: 0, name: '" + group.Name + "', isParent: true }";
*/

using System;
namespace Portal.Web.Core.Model
{
    public class ZTreeInfo
    {
        #region 01.字段
        /// <summary>
        /// 树类型，1表示一般树，2表示多选树，3表示单选树
        /// </summary>
        public int Type { get; set; }
        /// <summary>
        /// 数据来源类型
        /// </summary>
        public int DateType { get; set; }
        /// <summary>
        /// 树的名称
        /// </summary>
        public string TreeName { get; set; }
        /// <summary>
        /// Js名称
        /// </summary>
        public string JsName { get; set; }
        /// <summary>
        /// 树的数据源
        /// </summary>
        public string Source { get; set; }
        /// <summary>
        /// 公司ID
        /// </summary>
        public string CompanyId { get; set; }
        #endregion

        #region 02.初始化
        public ZTreeInfo()
        {
            JsName = "ShowMsg";
        }

        public ZTreeInfo(int datatype,int type,string jsname)
        {
            DateType = datatype;
            Type = type;
            JsName = jsname;
        }
        #endregion
    }
}
