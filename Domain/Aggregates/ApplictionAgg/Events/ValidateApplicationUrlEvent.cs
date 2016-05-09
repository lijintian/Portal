

using EasyDDD.Core.Event;

namespace Portal.Domain.Aggregates.ApplictionAgg.Events
{
    public class ValidateApplicationUrlEvent : DomainEvent
    {
        public string Url { get; set; }

        public ValidateApplicationUrlEvent(Application doamin, string url)
            : base(doamin)
        {
            this.Url = url;
        }
    }
}
