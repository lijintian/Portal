

using EasyDDD.Core.Event;

namespace Portal.Domain.Aggregates.ApplictionAgg.Events
{
    public class ValidateApplicationExistsSameEnNameEvent: DomainEvent
    {
         public string EnName { get; set; }

         public ValidateApplicationExistsSameEnNameEvent(Application application, string enName)
             : base(application)
        {
            this.EnName = enName;
        }
    }
}
