using Portal.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Web.Admin.Model.PermissionSetting
{
    public class ApplicationApiPermission
    {
        public Portal.Dto.Application APP { get; set; }
        public IEnumerable<ApiPermission> ApiPermissions { get; set; }
    }
}
