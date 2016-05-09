using System.Collections.Generic;
using System.Data;
using Portal.Dto;
using Portal.Dto.Request;

namespace Portal.Applications.Events
{
    public class ImportRoleCheckEvent : BaseImportCheckEvent<ImportRoleRequest>
    {
        public ImportRoleCheckEvent(List<ImportRoleRequest> list, DataTable errorTable, SysLoggerDto logger)
            : base(list, errorTable, logger)
        {
        }
    }
}
