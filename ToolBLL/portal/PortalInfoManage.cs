using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using System.Configuration;

namespace ToolBLL.portal
{
    public class PortalInfoManage : MongoDBHelper
    {
        static int beforeMinute = Convert.ToInt32(ConfigurationManager.AppSettings["BeforeMinute"]);
      
        public PortalInfoManage()
        {
        }
        public List<PortalInfo> GetPortalInfo()
        {
            DateTime updateOn = DateTime.Now.AddMinutes(beforeMinute);
            //获取表
            MongoCollection<PortalInfo> col = _database.GetCollection<PortalInfo>("SynchronizationInfo");
            return col.Find(Query.GTE("UpdateOn", updateOn)).ToList();
        }

    }
}
