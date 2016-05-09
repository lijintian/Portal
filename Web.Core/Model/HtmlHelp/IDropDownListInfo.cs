/*
 EasyDDD
系统名称：  Model
模块名称：  下拉数据源
创建日期：  2015-07-30

内容摘要：  OrderBy为整数类型
*/

namespace Portal.Web.Core.Model
{
    public class IDropDownListInfo : DropDownListinfo<int>
    {
        #region 01.初始化

        public IDropDownListInfo()
        {
        }

        public IDropDownListInfo(string id, string name)
            : base(id, name)
        {
        }

        public IDropDownListInfo(string id, string name, string parentid)
            : base(id, name, parentid)
        {
        }

        public IDropDownListInfo(string id, string name, string parentid, int orderby)
            : base(id, name, parentid, orderby)
        {
        }
        public IDropDownListInfo(string id, string name, string code, string parentid, int orderby)
            : base(id, name, code, parentid, orderby)
        {
        }
        #endregion

        #region 02.方法
        /// <summary>
        /// 复制
        /// </summary>
        /// <returns></returns>
        public override object Clone()
        {
            return MemberwiseClone();
        }
        #endregion

    }
}
