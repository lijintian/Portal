using System.Collections.Generic;
using System.Data;
using Portal.Dto;
using Portal.Dto.Request;

namespace Portal.Applications.Events
{
    public class ImportUserRoleCheckEvent : BaseImportCheckEvent<ImportUserRoleRequest>
    {
        #region 属性
        public bool IsCreate { get; private set; }
        #endregion

        #region 初始化
        public ImportUserRoleCheckEvent(List<ImportUserRoleRequest> list, DataTable errorTable, bool isCreate, SysLoggerDto logger)
            : base(list, errorTable, logger)
        {
            this.IsCreate = isCreate;
        }
        #endregion
    }
}
