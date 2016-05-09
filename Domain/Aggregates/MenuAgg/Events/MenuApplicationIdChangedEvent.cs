

using EasyDDD.Core.Event;

namespace Portal.Domain.Aggregates.MenuAgg.Events
{
    public class MenuApplicationIdChangedEvent : DomainEvent
    {
        /// <summary>
        /// 所属应用程序
        /// </summary>
        public string ApplicationId { get; private set; }

        public MenuApplicationIdChangedEvent(Menu menu,string appId)
            : base(menu)
        {
            this.ApplicationId = appId;
        }
    }
}
