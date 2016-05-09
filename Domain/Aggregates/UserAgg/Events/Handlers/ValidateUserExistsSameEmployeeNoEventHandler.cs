

using EasyDDD.Core.Event;
using EasyDDD.Infrastructure.Crosscutting.Helpers;
using Portal.Domain.Aggregates.UserAgg.Events.Callbacks;
using Portal.Domain.Repositories;
using Portal.Domain.Specification.User;
using System;

namespace Portal.Domain.Aggregates.UserAgg.Events.Handlers
{
    public class ValidateUserExistsSameEmployeeNoEventHandler : IDomainEventHandler<ValidateUserExistsSameEmployeeNoEvent>
    {
        private readonly IUserRepository _userRepository;
        public ValidateUserExistsSameEmployeeNoEventHandler(IUserRepository userRepository)
        {
            Check.Argument.IsNotNull(userRepository, "userRepository");
            this._userRepository = userRepository;
        }

        public void Handle(ValidateUserExistsSameEmployeeNoEvent domainEvent)
        {
            throw new NotImplementedException();//不需要实现， 因为需要CallBack
        }

        public void Handle<TDomainEventResult>(ValidateUserExistsSameEmployeeNoEvent domainEvent, Action<TDomainEventResult> callback) where TDomainEventResult : IDomainEventResult
        {
            Check.Argument.IsNotNull(domainEvent, "domainEvent");

            var user = _userRepository.Get(new EmployeeNoSpecification(domainEvent.EmployeeNo));
            bool exists = user != null && user.Id != domainEvent.Source.Id;
            var result = new ValidateUserExistsSameEmployeeNoEventResult(exists);
            if (callback != null)
            {
                callback((TDomainEventResult)(IDomainEventResult)result);
            }
        }
    }
}
