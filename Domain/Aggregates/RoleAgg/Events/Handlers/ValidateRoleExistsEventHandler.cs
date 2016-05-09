

using EasyDDD.Core.Event;
using EasyDDD.Infrastructure.Crosscutting.Helpers;
using Portal.Domain.Aggregates.RoleAgg.Events.Callbacks;
using Portal.Domain.Repositories;
using Portal.Domain.Specification.Role;
using System;
using System.Linq;

namespace Portal.Domain.Aggregates.RoleAgg.Events.Handlers
{
    public class ValidateRoleExistsEventHandler : IDomainEventHandler<ValidateRoleExistsEvent>
    {
        private readonly IRoleRepository _roleRepository;
        public ValidateRoleExistsEventHandler(IRoleRepository roleRepository)
        {
            Check.Argument.IsNotNull(roleRepository, "roleRepository");
            this._roleRepository = roleRepository;
        }

        public void Handle(ValidateRoleExistsEvent domainEvent)
        {
            throw new NotImplementedException();//不需要实现,因为需要CallBack
        }

        public void Handle<TDomainEventResult>(ValidateRoleExistsEvent domainEvent, Action<TDomainEventResult> callback) where TDomainEventResult : IDomainEventResult
        {
            Check.Argument.IsNotNull(domainEvent, "domainEvent");

            var roles = _roleRepository.GetList(new RoleCodeListSpecification(domainEvent.Codes.ToArray()));

            var noFoundRoles = domainEvent.Codes.Except(roles.Select(c => c.Code));

            var result = new ValidateRoleExistsEventResult(noFoundRoles.ToArray());
            if (callback != null)
            {
                callback((TDomainEventResult)(IDomainEventResult)result);
            }
        }
    }
}
