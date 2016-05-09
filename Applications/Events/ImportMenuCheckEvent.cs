using System.Collections.Generic;
using System.Data;
using Portal.Dto;
using Portal.Dto.Request;

namespace Portal.Applications.Events
{
    public class ImportMenuCheckEvent : BaseImportCheckEvent<ImportMenuRequest>
    {
        public ImportMenuCheckEvent(List<ImportMenuRequest> list, DataTable errorTable, SysLoggerDto logger)
            : base(list, errorTable, logger)
        {
        }
    }
}
