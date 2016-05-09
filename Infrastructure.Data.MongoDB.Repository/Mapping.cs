using System;
using System.Collections.Generic;

using Portal.Domain.Aggregates.PermissionAgg;
using MongoDB.Bson.Serialization;
using EasyDDD.Core;

namespace Portal.Infrastructure.Data.MongoDB.Repository
{
    public class Mapping : IMapping
    {
        public void Map()
        {
            //注册继承关系
            BsonClassMap.RegisterClassMap<PagePermission>();
            BsonClassMap.RegisterClassMap<FunctionPermission>();
            BsonClassMap.RegisterClassMap<ApiPermission>();
        }

        public Dictionary<Type, string> GetEntityVsTableNameMappingInfos()
        {
            var dic = new Dictionary<Type, string>();
            dic.Add(typeof(PagePermission),"Permission");
            dic.Add(typeof(FunctionPermission), "Permission");
            dic.Add(typeof(ApiPermission), "Permission");

            return dic;
        }
    }
}
