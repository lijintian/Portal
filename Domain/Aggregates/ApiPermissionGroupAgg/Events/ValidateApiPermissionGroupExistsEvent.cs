

using EasyDDD.Core.Event;

namespace Portal.Domain.Aggregates.ApiPermissionGroupAgg.Events
{
    public class ValidateApiPermissionGroupExistsEvent : DomainEvent
    {
        public string[] Codes { get; private set; }
        public ValidateApiPermissionGroupExistsEvent(string codes)
            : base(null)
        {
            this.Codes = codes.Split(',');
        }
    }
}
