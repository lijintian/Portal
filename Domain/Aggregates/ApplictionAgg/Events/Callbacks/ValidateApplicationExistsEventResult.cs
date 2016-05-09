

using EasyDDD.Core.Event;

namespace Portal.Domain.Aggregates.ApplictionAgg.Events.Callbacks
{
    public class ValidateApplicationExistsEventResult : IDomainEventResult
    {

        public bool Exists { get; private set; }

        public string Name { get; private set; }

        public ValidateApplicationExistsEventResult(bool exists, string name)
        {
            this.Exists = exists;
            this.Name = name;
        }
    }
}
