



using Portal.Domain.Aggregates.ApplictionAgg;
using Portal.Domain.Aggregates.ApplictionAgg.Events;
using Portal.Domain.Aggregates.ApplictionAgg.Events.Handlers;
using Portal.Domain.Repositories;
using Portal.Infrastructure.Exceptions;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;
using EasyDDD.Infrastructure.Crosscutting.InversionOfControl;
using EasyDDD.Core.IdGenerator;
using EasyDDD.Core.Event;
using EasyDDD.Core.Specification;

namespace Portal.Domain.Tests
{

    public class ApplicationTest
    {
        [Fact]
        public void Should_SetNameOk_When_NotExistsSameName()
        {
            var identityGeneratorMoq = new Mock<IIdentityGenerator>();
            identityGeneratorMoq.Setup(c => c.NewId()).Returns(Guid.NewGuid().ToString());

            var repository = new Mock<IApplicationRepository>();
            repository.Setup(c => c.Get(It.IsAny<ISpecification<Application>>())).Returns((Application)null);

            var resolverMoq = new Mock<IDependencyResolver>();
            resolverMoq.Setup(c => c.Resolve<IIdentityGenerator>()).Returns(identityGeneratorMoq.Object);
            resolverMoq.Setup(c => c.ResolveAll<IDomainEventHandler<ValidateApplicationExistsSameNameEvent>>())
                .Returns(new List<IDomainEventHandler<ValidateApplicationExistsSameNameEvent>>()
                {
                    new ValidateApplicationExistsSameNameEventHandler(repository.Object)
                });
            IoC.Initialize(resolverMoq.Object);

            var application = new Application("test", "EnName");

            Assert.Equal("test", application.Name);

        }

        [Fact]
        public void Should_ThrowException_When_ExistsSameName()
        {
            //var identityGeneratorMoq = new Mock<IIdentityGenerator>();
            //identityGeneratorMoq.Setup(c => c.NewId()).Returns(Guid.NewGuid().ToString());

            //var resolverMoq = new Mock<IDependencyResolver>();

            //IoC.Initialize(resolverMoq.Object);
           

            //resolverMoq.Setup(c => c.Resolve<IIdentityGenerator>()).Returns(identityGeneratorMoq.Object);
            //resolverMoq.Setup(c => c.ResolveAll<IDomainEventHandler<ValidateApplicationExistsSameNameEvent>>())
            //    .Returns(new List<IDomainEventHandler<ValidateApplicationExistsSameNameEvent>>()
            //    {
            //        new ValidateApplicationExistsSameNameEventHandler(repository.Object)
            //    });

            ////applicatonMoq.SetupGet(x => x.Id).Returns("1");





            //PortalException ex = Assert.Throws<PortalException>(() => new Application("test"));
            //Assert.NotNull(ex);
            //Assert.Equal(ErrorCodes.StringCodes.ExistsSameApplicationName, ex.ErrorCode);
        }
    }
}
