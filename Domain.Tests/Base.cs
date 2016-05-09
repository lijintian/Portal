using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;





using Portal.Domain.Aggregates.DeveloperAppAgg.Strategies;
using Portal.Domain.Aggregates.UserAgg;
using Portal.Domain.Aggregates.UserAgg.Events;
using Portal.Domain.Aggregates.UserAgg.Events.Handlers;
using Portal.Domain.Aggregates.UserAgg.Strategies;
using Portal.Domain.Repositories;
using Portal.Infrastructure.Data.MongoDB.Repository;
using Moq;
using EasyDDD.Core.IdGenerator;
using EasyDDD.Infrastructure.Crosscutting.InversionOfControl;
using EasyDDD.Core.Repository;
using EasyDDD.Infrastructure.Data.MongoDB;

namespace Portal.Domain.Tests
{
    public abstract class Base
    {
        protected Base()
        {
            var identityGeneratorMoq = new Mock<IIdentityGenerator>();
            identityGeneratorMoq.Setup(c => c.NewId()).Returns(Guid.NewGuid().ToString());

            var resolverMoq = new Mock<IDependencyResolver>();
            resolverMoq.Setup(c => c.Resolve<IIdentityGenerator>()).Returns(identityGeneratorMoq.Object);
            resolverMoq.Setup(c => c.Resolve<IRepositoryContext>()).Returns(() => new MongoDBRepositoryContext("PortalDB"));
            resolverMoq.Setup(c => c.Resolve<IDeveloperAppRepository>()).Returns(() => new DeveloperAppRepository(IoC.Resolve<IRepositoryContext>()));
            resolverMoq.Setup(c => c.Resolve<IAuthenticateTicketGenerateStategy>()).Returns(new AuthenticateTicketGenerateStategy());

            IoC.Initialize(resolverMoq.Object);
        }
    }
}
