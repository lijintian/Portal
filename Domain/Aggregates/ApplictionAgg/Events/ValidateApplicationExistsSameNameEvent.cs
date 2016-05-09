using EasyDDD.Core.Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;


namespace Portal.Domain.Aggregates.ApplictionAgg.Events
{
    public class ValidateApplicationExistsSameNameEvent : DomainEvent
    {
         public string Name { get; set; }

         public ValidateApplicationExistsSameNameEvent(Application application, string name)
             : base(application)
        {
            this.Name = name;
        }
    }
}
