using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Web.Admin.Model.PermissionSetting
{
    public class ClonePermission
    {
        public int userType { get; set; }
        public string FromUserLoginName { get; set; }
        public string ToUserLoginName { get; set; }
    }
}
