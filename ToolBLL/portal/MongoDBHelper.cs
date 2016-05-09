using MongoDB.Bson;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using System;
using System.Configuration;

namespace ToolBLL.portal
{
    public class MongoDBHelper
    {
         //连接字符串
        private static string strConn = ConfigurationManager.ConnectionStrings["PortalDB"].ToString();
        private static readonly string dbName = ConfigurationManager.AppSettings["PortalDBName"];
        private readonly MongoServer _server;
        public readonly MongoDatabase _database;
        public MongoDBHelper()
        {
            var mongoUrl = new MongoUrl(strConn);
            var mongoServerSettings = MongoServerSettings.FromUrl(mongoUrl);
            _server = new MongoServer(mongoServerSettings);//创建数据连接
            _database = _server.GetDatabase(dbName);  //获取指定数据库
        }
        public static void InitMongodb()
        {
            var conventionPack = new ConventionPack();
            conventionPack.Add(new NamedIdMemberConvention("id", "_id"));
            conventionPack.Add(new IgnoreExtraElementsConvention(true));
            conventionPack.Add(new StringObjectIdIdGeneratorConvention());
            //枚举序列化为字符串
            conventionPack.Add(new EnumRepresentationConvention(BsonType.String));

            ConventionRegistry.Register("DefaultConvention", conventionPack, t => true);
            
        }

    }
}
