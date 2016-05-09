

using EasyDDD.Core.Event;

namespace Portal.Domain.Aggregates.RoleAgg.Events
{
    public class ValidateRoleExistsEvent : DomainEvent
    {
        public string[] Codes { get; private set; }
        public ValidateRoleExistsEvent(string[] codes)
            : base(null)
        {
            this.Codes = codes;
        }
    }
}
