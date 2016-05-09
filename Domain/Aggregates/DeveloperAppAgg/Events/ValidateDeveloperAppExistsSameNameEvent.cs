

using EasyDDD.Core.Event;

namespace Portal.Domain.Aggregates.DeveloperAppAgg.Events
{
    public class ValidateDeveloperAppExistsSameNameEvent: DomainEvent
    {
        public string Name { get; private set; }

        public ValidateDeveloperAppExistsSameNameEvent(DeveloperApp app, string name)
            : base(app)
        {
            this.Name = name;
        }
    }
}
