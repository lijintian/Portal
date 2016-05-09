using EasyDDD.Core.Event;

namespace Portal.Domain.Aggregates.ApplictionAgg.Events.Callbacks
{
    public class ValidateApplicationExistsSameEnNameEventResult : IDomainEventResult
    {
        /// <summary>
        /// 表示是否存在相同的英文名称
        /// </summary>
        public bool Exists { get; private set; }

        public ValidateApplicationExistsSameEnNameEventResult(bool exists)
        {
            this.Exists = exists;
        }
    }
}
