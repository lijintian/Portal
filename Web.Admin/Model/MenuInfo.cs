using System;
using Portal.Dto;

namespace Portal.Web.Admin.Model
{
    [Serializable]
    public class MenuInfo
    {
        #region 字段
        /// <summary>
        /// 菜单ID
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 父菜单Id
        /// </summary>
        public string ParentId { get; set; }

        /// <summary>
        /// 链接
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 是否是绝对地址
        /// </summary>
        public bool IsAbsoluteUrl { get; set; }

        /// <summary>
        /// 打开方式，参考枚举：MenuTarget
        /// </summary>
        public int Target { get; set; }

        /// <summary>
        /// 样式名称
        /// </summary>
        public string ClassName { get; set; }

        /// <summary>
        /// 是选打开、激活
        /// </summary>
        public bool IsActive;
        #endregion

        #region 初始化
        public MenuInfo()
        {
        }
        public MenuInfo(Menu info)
        {
            Id = info.Id;
            Name = info.Name;
            ParentId = info.ParentId;
            Url = info.Url;
            IsAbsoluteUrl = info.IsAbsoluteUrl;
            Target = info.Target;
        }
        #endregion
    }
}
