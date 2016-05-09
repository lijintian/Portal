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
    public class EnumModel : ICloneable
    {
        #region 01.字段
        /// <summary>
        /// 名称
        /// </summary>
        public string Text { get; set; }
        /// <summary>
        /// 值
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// 英文值
        /// </summary>
        public string TextEg { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 等级ID（从1开始）
        /// </summary>
        public int LevelId { get; set; }

        /// <summary>
        /// 排序ID
        /// </summary>
        public int OrderId { get; set; }

        /// <summary>
        /// 是否为下拉组
        /// </summary>
        public bool IsGroup { get; set; }
        #endregion

        #region 02.初始化
        public EnumModel(object id, string name, string description, int levelid, int orderid = 0)
        {
            LevelId = levelid;
            InitInfo(id.ToString(), name, description, orderid);
        }
        public EnumModel(object id, string name, int orderid = 0)
        {
            InitInfo(id.ToString(), name, null, orderid);
        }
        public EnumModel()
        { }
        #endregion

        #region 03.方法
        public void InitInfo(string id, string name, string description, int orderid = 0)
        {
            Value = id;
            Text = name;
            OrderId = orderid;
            Description = description;
        }
        /// <summary>
        /// 复制
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            return MemberwiseClone();
        }
        #endregion
    }
}
