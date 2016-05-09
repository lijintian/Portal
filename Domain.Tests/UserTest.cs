using System.Runtime.InteropServices;





using Portal.Domain.Aggregates.ApplictionAgg;
using Portal.Domain.Aggregates.ApplictionAgg.Events;
using Portal.Domain.Aggregates.ApplictionAgg.Events.Handlers;
using Portal.Domain.Aggregates.UserAgg.Strategies;
using Portal.Domain.Repositories;
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
using EasyDDD.Core.IdGenerator;
using EasyDDD.Core.Specification;
using EasyDDD.Infrastructure.Crosscutting.InversionOfControl;
using EasyDDD.Core.Event;
using EasyDDD.Infrastructure.Data.MongoDB;

namespace Portal.Domain.Tests
{

    public class UserTest
    {
        public UserTest()
        {
            var identityGeneratorMoq = new Mock<IIdentityGenerator>();
            identityGeneratorMoq.Setup(c => c.NewId()).Returns(Guid.NewGuid().ToString());

            var repository = new Mock<IUserRepository>();
            repository.Setup(c => c.Get(It.IsAny<ISpecification<User>>())).Returns((User)null);

            var resolverMoq = new Mock<IDependencyResolver>();
            resolverMoq.Setup(c => c.Resolve<IIdentityGenerator>()).Returns(identityGeneratorMoq.Object);
            resolverMoq.Setup(c => c.Resolve<IPasswordEncryptStrategy>()).Returns(new PasswordEncryptStrategy());
            resolverMoq.Setup(c => c.ResolveAll<IDomainEventHandler<ValidateUserExistsSameLoginNameEvent>>())
                .Returns(new List<IDomainEventHandler<ValidateUserExistsSameLoginNameEvent>>()
                {
                    new ValidateUserExistsSameLoginNameEventHandler(repository.Object)
                });

            IoC.Initialize(resolverMoq.Object);
        }

        public void TestUp()
        {
            
        }

        [Fact]
        public void Should_IsApproved_True_When_Enable()
        {
           

            var user = new User("test");
            user.Enable();
            Assert.Equal(true,user.IsApproved);

        }

        [Fact]
        public void TestCreate()
        {
            var u = new User("WishTesting", "123456", UserType.ExternalApi);
            u.SetEmail("jianyong.jiang@chukou1.com");
            u.PhoneNumber = "123456778";

            var context = new MongoDBRepositoryContext("PortalDB");
            var rep = new UserRepository(context);
            rep.Add(u);
            context.Commit();
        }

    }
}
