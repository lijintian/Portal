
using EasyDDD.Core.Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Domain.Aggregates.MenuAgg.Events
{
    public class ValidateMenuExistsParentIdEvent : DomainEvent
    {
        /// <summary>
        /// 所属应用程序
        /// </summary>
        public string ApplicationId { get; private set; }
        public string ParentId { get; private set; }
        public ValidateMenuExistsParentIdEvent(Menu menu,string appId, string parentId)
            : base(menu)
        {
            this.ApplicationId = appId;
            this.ParentId = parentId;
        }
    }
}
