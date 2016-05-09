

using EasyDDD.Core.Event;

namespace Portal.Domain.Aggregates.DeveloperAppAgg.Events.Callbacks
{
    public class ValidateDeveloperAppExistsSameNameEventResult : IDomainEventResult
    {
        public bool Exists { get; set; }

        public ValidateDeveloperAppExistsSameNameEventResult(bool exists)
        {
            this.Exists = exists;
        }
    }
}
