/*
 EasyDDD
系统名称：  Model
模块名称：  下拉数据源
创建日期：  2015-07-30

内容摘要： 
*/
using System;

namespace Portal.Web.Core.Model
{
    public class DropDownListinfo<T>:ICloneable
    {
        #region 01.字段
        /// <summary>
        /// 主键
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 等级ID（从1开始）
        /// </summary>
        public int LevelId { get; set; }
        /// <summary>
        /// 父节点ID
        /// </summary>
        public string ParentId { get; set; }
        /// <summary>
        /// 父节点名称
        /// </summary>
        public string ParentName { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 排序ID
        /// </summary>
        public T OrderBy { get; set; }
        #endregion

        #region 02.初始化

        public DropDownListinfo()
        {
        }
        public DropDownListinfo(string id, string name)
        {
            Id = id;
            Name = name;
        }
        public DropDownListinfo(string id, string name, string parentid)
        {
            Id = id;
            Name = name;
            ParentId = parentid;
        }
        public DropDownListinfo(string id, string name, string parentid, T orderby)
        {
            Id = id;
            Name = name;
            ParentId = parentid;
            OrderBy = orderby;
        }
        public DropDownListinfo(string id, string name, string code, string parentid, T orderby)
        {
            Id = id;
            Name = string.Format("{0}{1}", name, string.IsNullOrEmpty(code) ? "" : string.Format("【{0}】", code));
            ParentId = parentid;
            OrderBy = orderby;
        }
        #endregion

        #region 03.方法
         /// <summary>
        /// 复制
        /// </summary>
        /// <returns></returns>
        public virtual object Clone()
        {
            return MemberwiseClone();
        }
        #endregion

    }
}
