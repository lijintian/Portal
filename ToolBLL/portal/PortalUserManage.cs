using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using System.Configuration;

namespace ToolBLL.portal
{
    public class PortalUserManage : MongoDBHelper
    {
        public PortalUserManage()
        {
        }
        public List<User> GetUser()
        {
            //获取表
            MongoCollection<User> col = _database.GetCollection<User>("User");

            return col.FindAll().ToList();
        }
        public User GetUserByLoginName(string loginName)
        {
            //获取表
            MongoCollection<User> col = _database.GetCollection<User>("User");

            return col.FindOne(Query.EQ("LoginName", loginName));
        }
        public User GetUserByID(string id)
        {
            //获取表
            MongoCollection<User> col = _database.GetCollection<User>("User");

            return col.FindOne(Query.EQ("_id", id));
        }

        public void InsertUser(User user)
        {
            //获取表
            var col = _database.GetCollection<User>("User");
            col.Insert(user);

        }
        public bool UpdateUser(User user)
        {
            var col = _database.GetCollection<User>("User");
            BsonDocument bsonDocument = BsonExtensionMethods.ToBsonDocument(user);
            bsonDocument.Remove("_id");
            IMongoQuery query = Query.EQ("_id", user._id);
            FindAndModifyArgs args = new FindAndModifyArgs()
            {
                Query = query,
                Update = new UpdateDocument(bsonDocument),

            };
            var result = col.FindAndModify(args);
            return result != null && result.Ok;
        }

        public Role GetRoleByCode(string code)
        {
            //获取表
            MongoCollection<Role> col = _database.GetCollection<Role>("Role");

            return col.FindOne(Query.EQ("Code", code));
        }


    }
}
