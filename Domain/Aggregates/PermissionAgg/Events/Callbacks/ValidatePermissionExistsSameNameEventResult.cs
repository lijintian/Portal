

using EasyDDD.Core.Event;

namespace Portal.Domain.Aggregates.PermissionAgg.Events.Callbacks
{
    public class ValidatePermissionExistsSameNameEventResult : IDomainEventResult
    {
          /// <summary>
        /// 表示是否存在相同的信息
        /// </summary>
        public bool Exists { get; private set; }

        public ValidatePermissionExistsSameNameEventResult(bool exists)
        {
            this.Exists = exists;
        }
    }
}
