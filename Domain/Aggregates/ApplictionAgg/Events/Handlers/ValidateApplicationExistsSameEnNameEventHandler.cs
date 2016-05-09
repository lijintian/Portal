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
    public class ValidateApplicationExistsSameEnNameEventHandler : IDomainEventHandler<ValidateApplicationExistsSameEnNameEvent>
    {
        private readonly IApplicationRepository _applicationRepository;
        public ValidateApplicationExistsSameEnNameEventHandler(IApplicationRepository applicationRepository)
        {
            Check.Argument.IsNotNull(applicationRepository, "applicationRepository");
            this._applicationRepository = applicationRepository;
        }

        public void Handle(ValidateApplicationExistsSameEnNameEvent domainEvent)
        {
            throw new NotImplementedException();//不需要实现， 因为需要CallBack
        }

        public void Handle<TDomainEventResult>(ValidateApplicationExistsSameEnNameEvent domainEvent, Action<TDomainEventResult> callback) where TDomainEventResult : IDomainEventResult
        {
            Check.Argument.IsNotNull(domainEvent, "domainEvent");

            var application = _applicationRepository.Get(new ApplicationEnNameSpecification(domainEvent.EnName));
            bool exists = application != null && application.Id != domainEvent.Source.Id;
            var result = new ValidateApplicationExistsSameEnNameEventResult(exists);
            if (callback != null)
            {
                callback((TDomainEventResult)(IDomainEventResult)result);
            }
        }
    }
}
