

using EasyDDD.Core.Event;

namespace Portal.Domain.Aggregates.MenuAgg.Events.Callbacks
{
    public class ValidateMenuExistsParentIdEventResult : IDomainEventResult
    {
          /// <summary>
        /// 是否存在ParentId
        /// </summary>
        public bool Exists { get; private set; }

        public ValidateMenuExistsParentIdEventResult(bool exists)
        {
            this.Exists = exists;
        }
    }
}
