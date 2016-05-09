using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Portal.Domain.Aggregates.PermissionAgg;
using Portal.Infrastructure.Data.MongoDB.Repository;
using Xunit;
using EasyDDD.Infrastructure.Data.MongoDB;

namespace Portal.Domain.Tests
{
    class PermissionTest : Base
    {
        [Fact]
        public void TestApiPermissionCreate()
        {
            var apiPer = new ApiPermission("55da945d7ff79d067c7d364e", "Add_New_Order", "允许创建新订单");
            apiPer.IsOpened = true;
            apiPer.IsCustomerGranted = true;

            var contex = new MongoDBRepositoryContext("PortalDB");
            var repo = new PermissionRepository(contex);
            repo.Add(apiPer);
            contex.Commit();
        }
    }
}
