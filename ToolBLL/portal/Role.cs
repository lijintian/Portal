using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolBLL.portal
{
    public class Role
    {
        public string _id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public List<string> Permissions { get; set; }

    }
}
