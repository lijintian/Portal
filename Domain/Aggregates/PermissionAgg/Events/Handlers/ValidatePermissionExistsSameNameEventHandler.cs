using System;



using Portal.Domain.Aggregates.PermissionAgg.Events.Callbacks;
using Portal.Domain.Repositories;
using Portal.Domain.Specification.Permission;
using EasyDDD.Core.Event;
using EasyDDD.Infrastructure.Crosscutting.Helpers;
using EasyDDD.Core.Specification;

namespace Portal.Domain.Aggregates.PermissionAgg.Events.Handlers
{
    public class ValidatePermissionExistsSameNameEventHandler : IDomainEventHandler<ValidatePermissionExistsSameNameEvent>
    {
        private readonly IPermissionRepository _permissionRepository;
        public ValidatePermissionExistsSameNameEventHandler(IPermissionRepository permissionRepository)
        {
            Check.Argument.IsNotNull(permissionRepository, "permissionRepository");
            this._permissionRepository = permissionRepository;
        }

        public void Handle(ValidatePermissionExistsSameNameEvent domainEvent)
        {
            throw new NotImplementedException();//不需要实现,因为需要CallBack
        }

        public void Handle<TDomainEventResult>(ValidatePermissionExistsSameNameEvent domainEvent, Action<TDomainEventResult> callback) where TDomainEventResult : IDomainEventResult
        {
            Check.Argument.IsNotNull(domainEvent, "domainEvent");
            ISpecification<Permission> specs = new PermissionApplicationIdSpecification(domainEvent.ApplicationId);
            specs = specs.And(new PermissionEqualNameSpecification(domainEvent.Name));
            specs = specs.And(new PermissionUnEqualIdSpecification(domainEvent.Source.Id));
            bool exists = _permissionRepository.Exists(domainEvent.IsApi, specs);
            var result = new ValidatePermissionExistsSameNameEventResult(exists);
            if (callback != null)
            {
                callback((TDomainEventResult)(IDomainEventResult)result);
            }
        }
    }
}
