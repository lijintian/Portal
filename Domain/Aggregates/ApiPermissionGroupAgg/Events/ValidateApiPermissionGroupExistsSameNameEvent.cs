

using EasyDDD.Core.Event;

namespace Portal.Domain.Aggregates.ApiPermissionGroupAgg.Events
{
    public class ValidateApiPermissionGroupExistsSameNameEvent : DomainEvent
    {
        public string Name { get; private set; }
        public string Code { get; private set; }

        public ValidateApiPermissionGroupExistsSameNameEvent(ApiPermissionGroup role, string code, string name)
            : base(role)
        {
            this.Name = name;
            this.Code = code;
        }
    }
}
