using System.ComponentModel;
using System.Data;

namespace Portal.Dto.Request
{
    public class ImportRoleRequest : BaseImportRequest
    {
        #region 属性
        [Description("应用系统英文名称")]
        public string ApplicationCode { get; set; }

        [Description("角色名称")]
        public string Name { get; set; }

        [Description("角色代码")]
        public string Code { get; set; }

        [Description("权限码")]
        public string PermissionCodes { get; set; }

        [Description("备注")]
        public string Desc { get; set; }
        #endregion
    }
}
