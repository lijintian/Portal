using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Portal.Dto
{
    /// <summary>
    /// 表示菜单传输对象
    /// </summary>
    public class Menu : DomainDto
    {
        #region 属性
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 父菜单Id
        /// </summary>
        public string ParentId { get; set; }
        /// <summary>
        /// 所属应用程序
        /// </summary>
        public string ApplicationId { get; set; }
        /// <summary>
        /// 打开方式，参考枚举：MenuTarget
        /// </summary>
        public int Target { get; set; }
        /// <summary>
        /// 是否共享（所有用户可见）
        /// </summary>
        public bool IsShare { get; set; }
        /// <summary>
        /// 链接
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// 是否是绝对地址
        /// </summary>
        public bool IsAbsoluteUrl { get; set; }
        /// <summary>
        /// 描述信息
        /// </summary>
        public string Desc { get; set; }
        /// <summary>
        /// 排列顺序
        /// </summary>
        public int Order { get; set; }
        /// <summary>
        /// 关联权限
        /// </summary>
        public string PermissionCode { get; set; }
        #endregion

        #region 方法
        /// <summary>
        /// 是否为一级节点
        /// </summary>
        /// <returns></returns>
        public bool IsFirstParent()
        {
            return IsFirstParent("0");
        }
        /// <summary>
        /// 是否为一级节点
        /// </summary>
        /// <param name="oldparentid">一级节点ID，如：0</param>
        /// <returns></returns>
        public bool IsFirstParent(string oldparentid)
        {
            return string.IsNullOrEmpty(this.ParentId) || this.ParentId == oldparentid;
        }
        /// <summary>
        /// 是否有子节点
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public bool HasChild(IEnumerable<Menu> list)
        {
            return list.Any(u => u.ParentId == this.Id);
        }
        /// <summary>
        /// 获取名称编码组合说明
        /// </summary>
        /// <returns></returns>
        public string GetNameCode()
        {
            return string.IsNullOrEmpty(this.PermissionCode) ? this.Name : string.Format("{0}[{1}]", this.Name, this.PermissionCode);
        }
        /// <summary>
        /// 设置绝对URL
        /// </summary>
        /// <param name="url"></param>
        public void SetUrl(string url)
        {
            if (this.IsAbsoluteUrl)
            {
                return;
            }
            this.Url = string.Format("{0}/{1}", url, this.Url.StartsWith("~/") ? this.Url.Replace("~/", "") : this.Url.StartsWith("../") ? this.Url.Replace("../", "") : this.Url.StartsWith("/") ? this.Url.TrimStart('/') : this.Url);
            this.IsAbsoluteUrl = true;
        }
        #endregion
    }

    public enum MenuTarget
    {
        [Description("当前窗口")]
        Self = 0,

        [Description("新窗口")]
        Blank = 1,
    }
}
