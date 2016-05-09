using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Web.Admin.Model
{
    [Serializable]
    public class LoginResponse
    {
        public string Token { get; set; }
        public string LoginName { get; set; }
        public string ReturnURL { get; set; }
        public int ReturnCode { get; set; }
        public string ErrorDescription { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}
