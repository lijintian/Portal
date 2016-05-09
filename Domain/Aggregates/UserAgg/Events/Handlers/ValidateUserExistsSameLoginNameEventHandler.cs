

using EasyDDD.Core.Event;
using EasyDDD.Infrastructure.Crosscutting.Helpers;
using Portal.Domain.Aggregates.UserAgg.Events.Callbacks;
using Portal.Domain.Repositories;
using Portal.Domain.Specification.User;
using System;

namespace Portal.Domain.Aggregates.UserAgg.Events.Handlers
{
    public class ValidateUserExistsSameLoginNameEventHandler : IDomainEventHandler<ValidateUserExistsSameLoginNameEvent>
    {
        private readonly IUserRepository _userRepository;
        public ValidateUserExistsSameLoginNameEventHandler(IUserRepository userRepository)
        {
            Check.Argument.IsNotNull(userRepository, "userRepository");
            this._userRepository = userRepository;
        }

        public void Handle(ValidateUserExistsSameLoginNameEvent domainEvent)
        {
            throw new NotImplementedException();//不需要实现， 因为需要CallBack
        }

        public void Handle<TDomainEventResult>(ValidateUserExistsSameLoginNameEvent domainEvent, Action<TDomainEventResult> callback) where TDomainEventResult : IDomainEventResult
        {
            Check.Argument.IsNotNull(domainEvent, "domainEvent");

            var application = _userRepository.Get(new UserLoginNameSpecification(domainEvent.LoginName).Or(new EmployeeNoSpecification(domainEvent.LoginName)));
            bool exists = application != null && application.Id != domainEvent.Source.Id;
            var result = new ValidateUserExistsSameLoginNameEventResult(exists);
            if (callback != null)
            {
                callback((TDomainEventResult)(IDomainEventResult)result);
            }
        }
    }
}
