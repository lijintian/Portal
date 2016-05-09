

using Portal.Domain.Aggregates.MenuAgg.Events.Callbacks;
using Portal.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Portal.Infrastructure.Exceptions;
using EasyDDD.Core.Event;
using EasyDDD.Infrastructure.Crosscutting.Helpers;

namespace Portal.Domain.Aggregates.MenuAgg.Events.Handlers
{
    public class ValidateMenuExistsParentIdEventHandler : IDomainEventHandler<ValidateMenuExistsParentIdEvent>
    {
        private readonly IMenuRepository _menuRepository;
        public ValidateMenuExistsParentIdEventHandler(IMenuRepository menuRepository)
        {
            Check.Argument.IsNotNull(menuRepository, "menuRepository");
            this._menuRepository = menuRepository;
        }

        public void Handle(ValidateMenuExistsParentIdEvent domainEvent)
        {
            throw new NotImplementedException();//不需要实现， 因为需要CallBack
        }

        public void Handle<TDomainEventResult>(ValidateMenuExistsParentIdEvent domainEvent, Action<TDomainEventResult> callback) where TDomainEventResult : IDomainEventResult
        {
            Check.Argument.IsNotNull(domainEvent, "domainEvent");

            var menu = _menuRepository.GetByKey(domainEvent.ParentId);
            bool exists = menu != null;
            if (exists && menu.ApplicationId != domainEvent.ApplicationId)
            {
                throw new PortalException(ErrorCodes.StringCodes.ApplicationNoFoundMenuId, ErrorMessage.ApplicationNoFoundMenuId.FormatWith(domainEvent.ParentId));
            }
            var result = new ValidateMenuExistsParentIdEventResult(exists);
            if (callback != null)
            {
                callback((TDomainEventResult)(IDomainEventResult)result);
            }
        }
    }
}
