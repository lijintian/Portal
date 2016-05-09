


using EasyDDD.Core.Event;
using EasyDDD.Core.Specification;
using EasyDDD.Infrastructure.Crosscutting.Helpers;
using Portal.Domain.Aggregates.RoleAgg.Events.Callbacks;
using Portal.Domain.Repositories;
using Portal.Domain.Specification.Role;
using System;

namespace Portal.Domain.Aggregates.RoleAgg.Events.Handlers
{
    public class ValidateRoleExistsSameNameEventHandler : IDomainEventHandler<ValidateRoleExistsSameNameEvent>
    {
        private readonly IRoleRepository _roleRepository;
        public ValidateRoleExistsSameNameEventHandler(IRoleRepository roleRepository)
        {
            Check.Argument.IsNotNull(roleRepository, "roleRepository");
            this._roleRepository = roleRepository;
        }

        public void Handle(ValidateRoleExistsSameNameEvent domainEvent)
        {
            throw new NotImplementedException();//不需要实现,因为需要CallBack
        }

        public void Handle<TDomainEventResult>(ValidateRoleExistsSameNameEvent domainEvent, Action<TDomainEventResult> callback) where TDomainEventResult : IDomainEventResult
        {
            Check.Argument.IsNotNull(domainEvent, "domainEvent");

            var role = _roleRepository.Get(new RoleCodeSpecification(domainEvent.Code));
            bool existsSameCode = role != null && role.Id != domainEvent.Source.Id;

            role = _roleRepository.Get(
                      new AndSpecification<Role>(new RoleApplicationIdSpecification(domainEvent.ApplicationId),
                                                 new RoleNameSpecification(domainEvent.Name))
                    );
            bool existsSameName = role != null && role.Id != domainEvent.Source.Id;
            var result = new ValidateRoleExistsSameNameEventResult(existsSameCode, existsSameName);
            if (callback != null)
            {
                callback((TDomainEventResult)(IDomainEventResult)result);
            }
        }
    }
}
