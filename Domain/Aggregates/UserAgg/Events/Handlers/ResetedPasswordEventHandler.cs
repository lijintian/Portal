

using EasyDDD.Core.Event;
using EasyDDD.Infrastructure.Crosscutting.Event;
using EasyDDD.Infrastructure.Crosscutting.Helpers;
using Portal.Domain.Aggregates.UserAgg.Events.Callbacks;
using Portal.Domain.Repositories;
using Portal.Domain.Specification.User;
using System;


namespace Portal.Domain.Aggregates.UserAgg.Events.Handlers
{
    public class ResetedPasswordEventHandler : IDomainEventHandler<ResetedPasswordEvent>
    {
        private readonly IEventBus _enEventBus;
        public ResetedPasswordEventHandler(IEventBus eventBus)
        {
            Check.Argument.IsNotNull(eventBus, "eventBus");
            this._enEventBus = eventBus;
        }

        public void Handle(ResetedPasswordEvent domainEvent)
        {
            _enEventBus.Publish<ResetedPasswordEvent>(domainEvent);
        }

        public void Handle<TDomainEventResult>(ResetedPasswordEvent domainEvent, Action<TDomainEventResult> callback) where TDomainEventResult : IDomainEventResult
        {
            throw new NotImplementedException();
        }
    }
}
