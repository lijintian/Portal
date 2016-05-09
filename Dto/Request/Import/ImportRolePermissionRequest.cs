using System.ComponentModel;

namespace Portal.Dto.Request
{
    public class ImportRolePermissionRequest : BaseImportRequest
    {
        #region 属性
        [Description("角色代码")]
        public string Code { get; set; }

        [Description("权限码")]
        public string PermissionCodes { get; set; }
        #endregion
    }
}
