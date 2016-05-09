using System;




using Portal.Domain.Aggregates.PermissionAgg;
using Portal.Infrastructure.Data.MongoDB.Repository;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Xunit;
using EasyDDD.Core.IdGenerator;
using EasyDDD.Infrastructure.Crosscutting.InversionOfControl;
using EasyDDD.Core;
using EasyDDD.Infrastructure.Data.MongoDB;

namespace Infrastructure.Data.MongoDB.Repository.Tests
{
    
    public class PermissionTest
    {
        [Fact]
        public void FindTest()
        {
           
            var identityGeneratorMoq = new Mock<IIdentityGenerator>();
            identityGeneratorMoq.Setup(c => c.NewId()).Returns(Guid.NewGuid().ToString());

            var resolverMoq = new Mock<IDependencyResolver>();
            resolverMoq.Setup(c => c.Resolve<IIdentityGenerator>()).Returns(identityGeneratorMoq.Object);
            resolverMoq.Setup(c => c.Resolve<IMapping>()).Returns(new Mapping());


            IoC.Initialize(resolverMoq.Object);
            MongoDBRepositoryContext context = new MongoDBRepositoryContext("PortalDB");

            var repository = new PermissionRepository(context);
            var list = repository.FindPagePermissionsInPage(1, 10, null, null);
        }

        [Fact]
        public void FindTest2()
        {

            var identityGeneratorMoq = new Mock<IIdentityGenerator>();
            identityGeneratorMoq.Setup(c => c.NewId()).Returns(Guid.NewGuid().ToString());

            var resolverMoq = new Mock<IDependencyResolver>();
            resolverMoq.Setup(c => c.Resolve<IIdentityGenerator>()).Returns(identityGeneratorMoq.Object);
            resolverMoq.Setup(c => c.Resolve<IMapping>()).Returns(new Mapping());


            IoC.Initialize(resolverMoq.Object);
            MongoDBRepositoryContext context = new MongoDBRepositoryContext("PortalDB");

            var repository = new PermissionRepository(context);
            var list = repository.FindApiPermissionsInPage(1, 10, null, null);
        }
    }
}
