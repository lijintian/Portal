using System.Collections.Generic;
using System.Data;
using Portal.Dto;
using Portal.Dto.Request;

namespace Portal.Applications.Events
{
    public class ImportPermissionCheckEvent : BaseImportCheckEvent<ImportPermissionRequest>
    {
        public ImportPermissionCheckEvent(List<ImportPermissionRequest> list, DataTable errorTable, SysLoggerDto logger)
            : base(list, errorTable, logger)
        {
        }
    }
}
