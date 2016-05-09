using System;


using Portal.Domain.Aggregates.DeveloperAppAgg.Events.Callbacks;
using Portal.Domain.Repositories;
using Portal.Domain.Specification.DeveloperApp;
using EasyDDD.Core.Event;
using EasyDDD.Infrastructure.Crosscutting.Helpers;

namespace Portal.Domain.Aggregates.DeveloperAppAgg.Events.Handlers
{
    public class ValidateDeveloperAppExistsSameNameEventHandler : IDomainEventHandler<ValidateDeveloperAppExistsSameNameEvent>
    {
        private readonly IDeveloperAppRepository _appRepository;
        public ValidateDeveloperAppExistsSameNameEventHandler(IDeveloperAppRepository appRepository)
        {
            Check.Argument.IsNotNull(appRepository, "appRepository");
            this._appRepository = appRepository;
        }

        public void Handle(ValidateDeveloperAppExistsSameNameEvent domainEvent)
        {
            throw new NotImplementedException();//不需要实现,因为需要CallBack
        }

        public void Handle<TDomainEventResult>(ValidateDeveloperAppExistsSameNameEvent domainEvent, Action<TDomainEventResult> callback) where TDomainEventResult : IDomainEventResult
        {
            Check.Argument.IsNotNull(domainEvent, "domainEvent");
            var source = domainEvent.Source as DeveloperApp;
            if (source == null) throw new NullReferenceException("ValidateDeveloperAppExistsSameNameEvent source is null");
            var app = _appRepository.Get(new DeveloperAppUserIdSpecification(source.UserId).And(new DeveloperAppNameSpecification(domainEvent.Name)));
            bool existsSameName = app != null && app.Id != domainEvent.Source.Id;
            var result = new ValidateDeveloperAppExistsSameNameEventResult(existsSameName);
            if (callback != null)
            {
                callback((TDomainEventResult)(IDomainEventResult)result);
            }
        }
    }
}
