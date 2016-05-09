



using Portal.Domain.Aggregates.ApplictionAgg;
using Portal.Domain.Aggregates.ApplictionAgg.Events;
using Portal.Domain.Aggregates.ApplictionAgg.Events.Handlers;
using Portal.Domain.Repositories;
using Portal.Infrastructure.Exceptions;
using Moq;
using System;
using System.Collections.Generic;
using Portal.Domain.Aggregates.UserAgg;
using Portal.Domain.Aggregates.UserAgg.Events;
using Portal.Domain.Aggregates.UserAgg.Events.Handlers;
using Xunit;
namespace Portal.Domain.Tests
{

    public class PagePermissionTest
    {
        [Fact]
        public void Should_IsApproved_True_When_Enable()
        {
            //var identityGeneratorMoq = new Mock<IIdentityGenerator>();
            //identityGeneratorMoq.Setup(c => c.NewId()).Returns(Guid.NewGuid().ToString());

            //var repository = new Mock<IUserRepository>();
            //repository.Setup(c => c.Get(It.IsAny<ISpecification<User>>())).Returns((User)null);

            //var resolverMoq = new Mock<IDependencyResolver>();
            //resolverMoq.Setup(c => c.Resolve<IIdentityGenerator>()).Returns(identityGeneratorMoq.Object);
            //resolverMoq.Setup(c => c.ResolveAll<IDomainEventHandler<ValidateUserExistsSameLoginNameEvent>>())
            //    .Returns(new List<IDomainEventHandler<ValidateUserExistsSameLoginNameEvent>>()
            //    {
            //        new ValidateUserExistsSameLoginNameEventHandler(repository.Object)
            //    });
            //IoC.Initialize(resolverMoq.Object);

            //var user = new User("test");
            //user.Enable();
            //Assert.Equal(true,user.IsApproved);

        }


    }
}
