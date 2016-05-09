using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolBLL.portal
{
   public class Menu
    {
       public Menu()
       {
           this._id = ObjectId.GenerateNewId().ToString();
       }
       public string _id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string ParentId { get;  set; }
        public string Desc { get; set; }
        public int Sequence { get; set; }
        public string PermissionCode { get;  set; }

        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public string _t { get; set; }
        public byte[] RowVersion { get; set; }
    }
}
