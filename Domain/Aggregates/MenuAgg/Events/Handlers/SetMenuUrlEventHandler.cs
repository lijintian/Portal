using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using Portal.Domain.Aggregates.MenuAgg.Events.Callbacks;
using Portal.Domain.Repositories;
using Portal.Domain.Specification.Permission;
using Portal.Infrastructure.Exceptions;
using EasyDDD.Core.Event;
using EasyDDD.Infrastructure.Crosscutting.Helpers;

namespace Portal.Domain.Aggregates.MenuAgg.Events.Handlers
{
    public class SetMenuUrlEventHandler : IDomainEventHandler<SetMenuUrlEvent>
    {
        private readonly IApplicationRepository _applicationRepository;
        private readonly IPermissionRepository _permissionRepository;
        public SetMenuUrlEventHandler(IApplicationRepository applicationRepository, IPermissionRepository persssionRepository)
        {
            Check.Argument.IsNotNull(applicationRepository, "applicationRepository");
            Check.Argument.IsNotNull(persssionRepository, "persssionRepository");
            this._applicationRepository = applicationRepository;
            this._permissionRepository = persssionRepository;
        }

        public void Handle(SetMenuUrlEvent domainEvent)
        {
            throw new NotImplementedException();//不需要实现， 因为需要CallBack
        }

        public void Handle<TDomainEventResult>(SetMenuUrlEvent domainEvent, Action<TDomainEventResult> callback) where TDomainEventResult : IDomainEventResult
        {
            Check.Argument.IsNotNull(domainEvent, "domainEvent");
            var application = _applicationRepository.GetByKey(domainEvent.ApplicationId);
            if (application == null)
            {
                throw new PortalException(ErrorCodes.StringCodes.NoFoundApplication, ErrorMessage.NoFoundApplication);
            }
            var result = new SetMenuUrlEventResult(application.Url);
            if (callback != null)
            {
                callback((TDomainEventResult)(IDomainEventResult)result);
            }
        }
    }
}
