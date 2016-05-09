using System.ComponentModel;

namespace Portal.Dto.Request
{
    public class ImportCustomerPermissionRequest : BaseImportRequest
    {
        #region 属性
        [Description("客户代码")]
        public string ClientNo { get; set; }

        [Description("权限码")]
        public string PermissionCodes { get; set; }
        #endregion
    }
}
