using System.ComponentModel;
using System.Data;

namespace Portal.Dto.Request
{
    public class ImportUserPermissionRequest : BaseImportRequest
    {
        #region 属性
        [Description("用户名称")]
        public string LoginName { get; set; }

        [Description("权限码")]
        public string PermissionCodes { get; set; }
        #endregion
    }
}
