using System.ComponentModel;
using System.Data;

namespace Portal.Dto.Request
{
    public class ImportUserRoleRequest : BaseImportRequest
    {
        #region 属性
        [Description("用户名称")]
        public string LoginName { get; set; }

        [Description("角色代码")]
        public string RoleCodes { get; set; }
        #endregion
    }
}
