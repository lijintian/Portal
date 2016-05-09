using EasyDDD.Core.Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;


namespace Portal.Domain.Aggregates.ApplictionAgg.Events
{
    public class ValidateApplicationExistsEvent : DomainEvent
    {
         public string ApplicationId { get; set; }

         public ValidateApplicationExistsEvent(string applicationId)
             : base(null)
        {
            this.ApplicationId = applicationId;
        }
    }
}
