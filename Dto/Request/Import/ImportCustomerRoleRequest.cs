using System.ComponentModel;

namespace Portal.Dto.Request
{
    public class ImportCustomerRoleRequest : BaseImportRequest
    {
        #region 属性
        [Description("客户代码")]
        public string ClientNo { get; set; }

        [Description("角色代码")]
        public string RoleCodes { get; set; }
        #endregion
    }
}
