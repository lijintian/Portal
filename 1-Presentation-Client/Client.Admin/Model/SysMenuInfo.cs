using System;
using System.Collections.Generic;
using System.Linq;

namespace Portal.Client.Model
{
    public class SysMenuInfo
    {
        #region 属性
        /// <summary>
        /// 菜单名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 菜单编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 路径
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 样式名称
        /// </summary>
        public string ClassName { get; set; }

        /// <summary>
        /// 子节点
        /// </summary>
        public List<SysMenuInfo> Childs { get; set; }
        #endregion

        #region 初始化
        public SysMenuInfo()
        { }
        public SysMenuInfo(string name)
        {
            this.Name = name;
            this.Childs = new List<SysMenuInfo>();
        }
        public SysMenuInfo(string name, string code, string currentCode)
            : this(name)
        {
            this.Code = code;
            this.Url = string.Format("~/About/Index?Code={0}", code);
            this.ClassName = string.Equals(code, currentCode, StringComparison.CurrentCultureIgnoreCase) ? "active" : "";
        }
        #endregion

        #region 方法
        public void SetClassName()
        {
            bool check = this.Childs != null && this.Childs.Any(u => !string.IsNullOrEmpty(u.ClassName));
            if (check)
            {
                this.ClassName = check ? "active" : "";
            }
        }
        #endregion
    }
}
