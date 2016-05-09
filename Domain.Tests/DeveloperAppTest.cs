using System.Runtime.InteropServices;





using Portal.Domain.Aggregates.ApplictionAgg;
using Portal.Domain.Aggregates.ApplictionAgg.Events;
using Portal.Domain.Aggregates.ApplictionAgg.Events.Handlers;
using Portal.Domain.Aggregates.DeveloperAppAgg;
using Portal.Domain.Aggregates.UserAgg.Strategies;
using Portal.Domain.Model;
using Portal.Domain.Repositories;
using Portal.Domain.Specification.DeveloperApp;
using Portal.Infrastructure.Data.MongoDB;
using Portal.Infrastructure.Data.MongoDB.Repository;
using Portal.Infrastructure.Exceptions;
using Moq;
using System;
using System.Collections.Generic;
using Portal.Domain.Aggregates.UserAgg;
using Portal.Domain.Aggregates.UserAgg.Events;
using Portal.Domain.Aggregates.UserAgg.Events.Handlers;
using Xunit;
using EasyDDD.Infrastructure.Data.MongoDB;

namespace Portal.Domain.Tests
{
    class DeveloperAppTest : Base
    {
        public DeveloperAppTest() : base()
        {
            
        }

        [Fact]
        public void TestCreate()
        {
            var app = new DeveloperApp("WishTesting", "WishTestingApp", "http://www.wish.com", true){ Desc = "test for wishing"};

            var contex = new MongoDBRepositoryContext("PortalDB");
            var repo = new DeveloperAppRepository(contex);
            repo.Add(app);
            contex.Commit();

        }

        [Fact]
        public void TestUpdate()
        {
            var contex = new MongoDBRepositoryContext("PortalDB");
            var repo = new DeveloperAppRepository(contex);
            var app = repo.Get(new DeveloperAppClientIdSpecification("7f4a5861-93a0-4380-b3b3-fbeb72a5e2e4"));
            //app.AddGroupRequestPermissions(new []
            //{
            //    new ReferenceApiPermssionInfo("Add_New_Order", true, true), 
            //});

            app.SubmitToApprove();

            repo.Update(app);
            contex.Commit();
        }
    }
}
