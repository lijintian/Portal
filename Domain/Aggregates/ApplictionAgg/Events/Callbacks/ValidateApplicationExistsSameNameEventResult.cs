using EasyDDD.Core.Event;

namespace Portal.Domain.Aggregates.ApplictionAgg.Events.Callbacks
{
    public class ValidateApplicationExistsSameNameEventResult : IDomainEventResult
    {
        /// <summary>
        /// 表示是否存在相同的名称
        /// </summary>
        public bool Exists { get; private set; }

        public ValidateApplicationExistsSameNameEventResult(bool exists)
        {
            this.Exists = exists;
        }
    }
}
