using System.Collections.Generic;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

namespace ToolBLL.portal
{
    public class PortalApplicationManage : MongoDBHelper
    {
        public Application GetByKey(string id)
        {
            List<Application> list = new List<Application>();
            //获取表
            MongoCollection<Application> col = _database.GetCollection<Application>("Application");
            return col.FindOne(Query.EQ("_id", id));
        }
    }
}
