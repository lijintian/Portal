using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Web.Admin.Model.Application
{
   public class ApplicationVM
    {
       public IEnumerable<Portal.Dto.Application> list { get; set; }
       public bool IsCanAdd { get; set; }
       public bool IsCanEdit { get; set; }
    }
}
