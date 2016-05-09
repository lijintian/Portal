using System.ComponentModel;
using System.Data;

namespace Portal.Dto.Request
{
    public class ImportPermissionRequest : BaseImportRequest
    {
        #region 属性
        [Description("应用系统英文名称")]
        public string ApplicationCode { get; set; }

        [Description("名称")]
        public string Name { get; set; }

        [Description("权限码")]
        public string Code { get; set; }

        /// <summary>
        /// 是否兼容老的业务系统
        /// </summary>
        [Description("兼容老系统")]
        public string IsCompatible { get; set; }

        /// <summary>
        /// 序列编号
        /// </summary>
        [Description("排序")]
        public string Order { get; set; }

        [Description("功能权限")]
        public string FunctionPermissions { get; set; }

        [Description("描述")]
        public string Desc { get; set; }
        #endregion
    }
}
