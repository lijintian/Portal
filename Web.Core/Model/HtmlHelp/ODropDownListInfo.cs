/*
 EasyDDD
系统名称：  Model
模块名称：  下拉数据源
创建日期：  2015-07-30

内容摘要： 
*/
namespace Portal.Web.Core.Model
{
    public class DropDownListInfo2<T,T2> : DropDownListinfo<T2>
    {
        #region 01.字段
        public T OtherInfo { get; set; }
        #endregion

        #region 02.初始化

        public DropDownListInfo2()
        {
        }

        public DropDownListInfo2(string id, string name, string parentid)
            : base(id, name, parentid)
        {
        }

        public DropDownListInfo2(string id, string name)
            : base(id, name)
        {
        }
        #endregion

        #region 03.方法
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
