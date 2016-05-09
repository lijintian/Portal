

using EasyDDD.Core.Event;

namespace Portal.Domain.Aggregates.UserAgg.Events.Callbacks
{
    public class ValidateUserExistsSameEmployeeNoEventResult : IDomainEventResult
    {
        /// <summary>
        /// 表示是否存在相同员工号
        /// </summary>
        public bool Exists { get; private set; }

        public ValidateUserExistsSameEmployeeNoEventResult(bool exists)
        {
            this.Exists = exists;
        }
    }
}
