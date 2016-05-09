using System;
using System.Linq;


using Portal.Domain.Repositories;
using Portal.Domain.Specification.Menu;
using EasyDDD.Core.Event;
using EasyDDD.Infrastructure.Crosscutting.Helpers;

namespace Portal.Domain.Aggregates.MenuAgg.Events.Handlers
{
    public class MenuApplicationIdChangedEventHandler : IDomainEventHandler<MenuApplicationIdChangedEvent>
    {
        #region 初始化
        private readonly IMenuRepository _menuRepository;
        public MenuApplicationIdChangedEventHandler(IMenuRepository menuRepository)
        {
            Check.Argument.IsNotNull(menuRepository, "menuRepository");
            this._menuRepository = menuRepository;
        }
        #endregion

        #region 方法
        public void Handle(MenuApplicationIdChangedEvent domainEvent)
        {
            SetApplicationId(domainEvent.Source.Id, domainEvent.ApplicationId);
        }

        public void Handle<TDomainEventResult>(MenuApplicationIdChangedEvent domainEvent, Action<TDomainEventResult> callback) where TDomainEventResult : IDomainEventResult
        {
            throw new NotImplementedException();//不需要实现， 因为需要CallBack
        }
        #endregion

        #region 私有方法
        private void SetApplicationId(string id, string applicationId)
        {
            var menuList = _menuRepository.GetList(new MenuParentIdSpecification(id));
            if (menuList != null && menuList.Any())
            {
                foreach (var item in menuList)
                {
                    item.SetChildApplicationId(applicationId);
                    _menuRepository.Update(item);
                    SetApplicationId(item.Id, applicationId);
                }
            }
        }
        #endregion
    }
}
