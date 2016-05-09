using System;
using System.Collections.Generic;
using Portal.Dto;
using Portal.SDK.Security;

namespace Portal.Web.Admin.Model
{
    /// <summary>
    /// 顶级菜单
    /// </summary>
    [Serializable]
    public class NavMenuInfo
    {
        #region 01.字段
        /// <summary>
        /// 当前菜单信息
        /// </summary>
        public MenuInfo Current { get; set; }
        /// <summary>
        /// 子节点个数
        /// </summary>
        public int ChildCount { get; set; }
        /// <summary>
        /// 节节点
        /// </summary>
        public List<NavMenuInfo> Child { get; set; }

        /// <summary>
        /// 是否选中
        /// </summary>
        public bool IsActive;
        #endregion

        #region 02.初始化
        public NavMenuInfo()
        {
        }

        public NavMenuInfo(MenuInfo info)
        {
            if (!string.IsNullOrEmpty(info.Url) && info.Target == (int)MenuTarget.Self)
            {
                info.Url = string.Format("{0}{1}{2}=1", info.Url, info.Url.IndexOf("?") < 0 ? "?" : "&", CK1PortalAuthenticationConfig.PortalFrameName);
            }
            this.Current = info;
        }
        //public NavMenuInfo(MenuInfo info, List<NavMenuInfo> childList)
        //{
        //    this.Current = info;
        //    this.ChildCount = childList.Count;
        //    this.Child = childList;
        //}
        #endregion
    }
}
