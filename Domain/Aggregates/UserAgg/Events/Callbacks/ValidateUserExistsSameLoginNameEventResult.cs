
using EasyDDD.Core.Event;

namespace Portal.Domain.Aggregates.UserAgg.Events.Callbacks
{
    public class ValidateUserExistsSameLoginNameEventResult : IDomainEventResult
    {
        /// <summary>
        /// 表示是否存在相同的登录用户名称
        /// </summary>
        public bool Exists { get; private set; }

        public ValidateUserExistsSameLoginNameEventResult(bool exists)
        {
            this.Exists = exists;
        }
    }
}
