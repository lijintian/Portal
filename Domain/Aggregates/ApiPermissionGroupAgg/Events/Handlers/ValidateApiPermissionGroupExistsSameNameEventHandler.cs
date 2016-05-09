using EasyDDD.Core.Event;
using EasyDDD.Infrastructure.Crosscutting.Helpers;
using Portal.Domain.Aggregates.ApiPermissionGroupAgg.Events.Callbacks;
using Portal.Domain.Repositories;
using Portal.Domain.Specification.ApiPermissionGroup;
using System;

namespace Portal.Domain.Aggregates.ApiPermissionGroupAgg.Events.Handlers
{
    public class ValidateApiPermissionGroupExistsSameNameEventHandler : IDomainEventHandler<ValidateApiPermissionGroupExistsSameNameEvent>
    {
        private readonly IApiPermissionGroupRepository _apiGroupRepository;
        public ValidateApiPermissionGroupExistsSameNameEventHandler(IApiPermissionGroupRepository apiGroupRepository)
        {
            Check.Argument.IsNotNull(apiGroupRepository, "apiGroupRepository");
            this._apiGroupRepository = apiGroupRepository;
        }

        public void Handle(ValidateApiPermissionGroupExistsSameNameEvent domainEvent)
        {
            throw new NotImplementedException();//不需要实现,因为需要CallBack
        }

        public void Handle<TDomainEventResult>(ValidateApiPermissionGroupExistsSameNameEvent domainEvent, Action<TDomainEventResult> callback) where TDomainEventResult : IDomainEventResult
        {
            Check.Argument.IsNotNull(domainEvent, "domainEvent");

            var group = _apiGroupRepository.Get(new ApiPermissionGroupCodeSpecification(domainEvent.Code));
            bool existsSameCode = group != null && group.Id != domainEvent.Source.Id;

            group = _apiGroupRepository.Get(new ApiPermissionGroupNameSpecification(domainEvent.Name));
            bool existsSameName = group != null && group.Id != domainEvent.Source.Id;
            var result = new ValidateApiPermissionGroupExistsSameNameEventResult(existsSameCode, existsSameName);
            if (callback != null)
            {
                callback((TDomainEventResult)(IDomainEventResult)result);
            }
        }
    }
}
