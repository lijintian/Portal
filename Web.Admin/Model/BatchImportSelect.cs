using Portal.Dto.Request.Enum;
using Portal.Web.Admin.Core;

namespace Portal.Web.Admin.Model
{
    public class BatchImportSelect
    {
        #region 属性
        /// <summary>
        /// 类型
        /// </summary>
        public TemplateType Type { get; set; }

        /// <summary>
        /// 是否有权限
        /// </summary>
        public bool HasPermission { get; set; }
        #endregion

        #region 初始化
        public BatchImportSelect()
        { }

        public BatchImportSelect(TemplateType type, string permissionCode)
        {
            this.Type = type;
            this.HasPermission = PageUtility.HasPermission(permissionCode);
        }
        #endregion
    }
}
