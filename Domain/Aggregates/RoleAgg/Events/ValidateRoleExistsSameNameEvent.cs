

using EasyDDD.Core.Event;

namespace Portal.Domain.Aggregates.RoleAgg.Events
{
    public class ValidateRoleExistsSameNameEvent : DomainEvent
    {
        public string Name { get; private set; }
        public string Code { get; private set; }
        public string ApplicationId { get; private set; }

        public ValidateRoleExistsSameNameEvent(Role role, string applicationId, string code, string name)
            : base(role)
        {
            this.ApplicationId = applicationId;
            this.Name = name;
            this.Code = code;
        }
    }
}
