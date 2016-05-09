using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Portal.Domain.Aggregates.ApplictionAgg.Events.Callbacks;
using Portal.Domain.Repositories;
using Portal.Domain.Specification.Application;
using EasyDDD.Core.Event;
using EasyDDD.Infrastructure.Crosscutting.Helpers;

namespace Portal.Domain.Aggregates.ApplictionAgg.Events.Handlers
{
    public class ValidateApplicationExistsEventHandler : IDomainEventHandler<ValidateApplicationExistsEvent>
    {
        private readonly IApplicationRepository _applicationRepository;
        public ValidateApplicationExistsEventHandler(IApplicationRepository applicationRepository)
        {
            Check.Argument.IsNotNull(applicationRepository, "applicationRepository");
            this._applicationRepository = applicationRepository;
        }

        public void Handle(ValidateApplicationExistsEvent domainEvent)
        {
            throw new NotImplementedException();//不需要实现， 因为需要CallBack
        }

        public void Handle<TDomainEventResult>(ValidateApplicationExistsEvent domainEvent, Action<TDomainEventResult> callback) where TDomainEventResult : IDomainEventResult
        {
            Check.Argument.IsNotNull(domainEvent, "domainEvent");

            var application = _applicationRepository.GetByKey(domainEvent.ApplicationId);
            bool exists = application != null;
            var result = new ValidateApplicationExistsEventResult(exists, exists ? application.Name : string.Empty);
            if (callback != null)
            {
                callback((TDomainEventResult)(IDomainEventResult)result);
            }
        }
    }
}
