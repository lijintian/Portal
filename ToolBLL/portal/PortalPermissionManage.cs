using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolBLL.portal
{
    public class PortalPermissionManage : MongoDBHelper
    {
        public PortalPermissionManage()
        {
        }
        public List<Permission> GetPermission()
        {
            //获取表
            MongoCollection<Permission> col = _database.GetCollection<Permission>("Permission");

            return col.FindAll().ToList();
        }
        public Permission GetPermissionByCode(string code)
        {
            List<Permission> list = new List<Permission>();
            //获取表
            MongoCollection<Permission> col = _database.GetCollection<Permission>("Permission");

            return col.FindOne(Query.EQ("Code", code));
        }
        public Permission GetPermissionByID(string id)
        {
            List<Permission> list = new List<Permission>();
            //获取表
            MongoCollection<Permission> col = _database.GetCollection<Permission>("Permission");

            return col.FindOne(Query.EQ("_id", id));
        }
        /// <summary>
        /// 判断权限码是否存在
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public bool HasPermissionCode(string code)
        {
            MongoCollection<Permission> col = _database.GetCollection<Permission>("Permission");
            return col.FindOne(Query.EQ("Code", code)) == null;
        }
        public void InsertPermission(Permission permission)
        {
            //获取表
            var col = _database.GetCollection<Permission>("Permission");
            col.Insert(permission);

        }
        public void UpdatePermission(Permission permission)
        {
            if (string.IsNullOrEmpty(permission.Code))
                return;
            //获取表
            var col = _database.GetCollection<Permission>("Permission");
            BsonDocument bsonDocument = BsonExtensionMethods.ToBsonDocument(permission);
            IMongoQuery query = Query.EQ("Code", permission.Code);
            col.Update(query, new UpdateDocument(bsonDocument));

        }
    }
}
