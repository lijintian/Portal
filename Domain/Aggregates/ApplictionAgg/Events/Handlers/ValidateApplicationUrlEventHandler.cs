using System;
using System.Linq;


using Portal.Domain.Repositories;
using Portal.Domain.Specification.Menu;
using EasyDDD.Core.Event;
using EasyDDD.Infrastructure.Crosscutting.Helpers;

namespace Portal.Domain.Aggregates.ApplictionAgg.Events.Handlers
{
    public class ValidateApplicationUrlEventHandler : IDomainEventHandler<ValidateApplicationUrlEvent>
    {
        private readonly IPermissionRepository _permissionRepository;
        private readonly IMenuRepository _menuRepository;
        public ValidateApplicationUrlEventHandler(IPermissionRepository permissionRepository, IMenuRepository menuRepository)
        {
            Check.Argument.IsNotNull(permissionRepository, "permissionRepository");
            Check.Argument.IsNotNull(menuRepository, "menuRepository");
            this._permissionRepository = permissionRepository;
            this._menuRepository = menuRepository;
        }

        public void Handle(ValidateApplicationUrlEvent domainEvent)
        {
            Check.Argument.IsNotNull(domainEvent, "domainEvent");
            Application domain = domainEvent.Source as Application;
            var permissonList = _permissionRepository.GetPagePermissionList(domain.Id);
            if (permissonList != null && permissonList.Any())
            {
                string[] idList = permissonList.Select(u => u.Code).ToArray();
                var menuList = _menuRepository.GetList(new MenuPermissionCodeListSpecification(idList));
                if (menuList != null)
                {
                    foreach (var menu in menuList)
                    {
                        if (!string.IsNullOrEmpty(menu.Url) && menu.Url.StartsWith(domain.Url))
                        {
                            menu.Url = menu.Url.Replace(domain.Url, domainEvent.Url);
                            _menuRepository.Update(menu);
                        }
                    }
                }
            }
        }

        public void Handle<TDomainEventResult>(ValidateApplicationUrlEvent domainEvent, Action<TDomainEventResult> callback) where TDomainEventResult : IDomainEventResult
        {
            throw new NotImplementedException();//不需要实现， 因为需要CallBack
        }
    }
}
