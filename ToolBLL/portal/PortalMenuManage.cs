using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolBLL.portal
{
   public class PortalMenuManage:MongoDBHelper
    {
       public void InsertMenu(Menu menu)
       {
           if (string.IsNullOrEmpty(menu.Name))
               return;
           //获取表
           var col = _database.GetCollection<Menu>("Menu");
           col.Insert(menu);

       }
    }
}
