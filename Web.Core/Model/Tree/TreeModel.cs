/*
 EasyDDD
系统名称：  Model
模块名称：  树的具体数据类
创建日期：  2015-07-29

内容摘要：  此类为格式化成用户树所需的内容: "{ id: " + group.ID + ", pId: 0, name: '" + group.Name + "', isParent: true }";
*/

using System;

namespace Portal.Web.Core.Model
{
    public class TreeModel
    {
        #region 字段
        /// <summary>
        /// 用户id，或者组id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 父id(即所属组的id)
        /// </summary>
        public string ParentId { get; set; }

        /// <summary>
        /// 是否组(组为true，用户为false)
        /// </summary>
        public bool IsGroup { get; set; }

        /// <summary>
        /// 名称，用户名或者组名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 是否是父级
        /// </summary>
        public bool IsParent { get; set; }

        /// <summary>
        /// 属性
        /// </summary>
        public string Rel { get; set; }

        /// <summary>
        /// Url属性
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 是否打开
        /// </summary>
        public bool Open { get; set; }

        /// <summary>
        /// 是否选中
        /// </summary>
        public bool Checked { get; set; }

        /// <summary>
        /// 图标
        /// </summary>
        public string Icon { get; set; }
        #endregion

        #region 初始化
        public TreeModel()
        {
        }
        public TreeModel(string id, string name, string pid, bool isparent)
        {
            InitInfo(id, name, pid, isparent, false);
        }
        public TreeModel(string id, string name, string pid, bool isparent, bool ischecked)
        {
            InitInfo(id, name, pid, isparent, ischecked);
        }
        public TreeModel(Guid id, string name, Guid pid)
        {
            InitInfo(id.ToString(), name, pid.ToString(), false, false);
        }
        public TreeModel(Guid id, string name, Guid pid, bool isparent)
        {
            InitInfo(id.ToString(), name, pid == Guid.Empty ? "0" : pid.ToString(), isparent, false);
        }
        public TreeModel(Guid id, string name, Guid pid, bool isparent, bool ischecked)
        {
            InitInfo(id.ToString(), name, pid == Guid.Empty ? "0" : pid.ToString(), isparent, ischecked);
        }
        public TreeModel(Guid id, string name, Guid pid, Guid firstParentId, bool isparent, bool ischecked)
        {
            InitInfo(id.ToString(), name, pid == firstParentId ? "0" : pid.ToString(), isparent, ischecked);
        }
        #endregion

        #region 方法
        public void InitInfo(string id, string name, string pid, bool isparent, bool ischecked)
        {
            this.Id = id;
            this.Name = name;
            this.ParentId = pid;
            this.IsParent = isparent;
            this.Checked = ischecked;
        }
        #endregion

    }
}
