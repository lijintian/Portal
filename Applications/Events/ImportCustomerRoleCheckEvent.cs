using System.Collections.Generic;
using System.Data;
using Portal.Dto;
using Portal.Dto.Request;

namespace Portal.Applications.Events
{
    public class ImportCustomerRoleCheckEvent : BaseImportCheckEvent<ImportCustomerRoleRequest>
    {
        #region 属性
        public bool IsCreate { get; private set; }
        #endregion

        #region 初始化
        public ImportCustomerRoleCheckEvent(List<ImportCustomerRoleRequest> list, DataTable errorTable, bool isCreate, SysLoggerDto logger)
            : base(list, errorTable, logger)
        {
            this.IsCreate = isCreate;
        }
        #endregion
    }
}
