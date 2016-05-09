using System.ComponentModel;

namespace Portal.Dto.Request
{
    public class ImportMenuRequest : BaseImportRequest
    {
        #region 属性
        [Description("应用系统英文名称")]
        public string ApplicationCode { get; set; }
        /// <summary>
        /// 父菜单Id
        /// </summary>
        [Description("上级菜单ID")]
        public string ParentId { get; set; }

        [Description("菜单名称")]
        public string Name { get; set; }

        [Description("权限码")]
        public string PermissionCode { get; set; }

        /// <summary>
        /// 链接
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 打开方式，参考枚举：MenuTarget
        /// </summary>
        [Description("打开方式")]
        public string Target { get; set; }

        /// <summary>
        /// 是否共享（所有用户可见）
        /// </summary>
        [Description("共享")]
        public string IsShare { get; set; }

        /// <summary>
        /// 序列编号
        /// </summary>
        [Description("排序")]
        public string Order { get; set; }

        [Description("备注")]
        public string Desc { get; set; }
        #endregion
    }
}
