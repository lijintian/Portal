

using EasyDDD.Core.Event;

namespace Portal.Domain.Aggregates.ApiPermissionGroupAgg.Events.Callbacks
{
    public class ValidateApiPermissionGroupExistsSameNameEventResult : IDomainEventResult
    {
        /// <summary>
        /// 表示是否存在相同的角色Code
        /// </summary>
        public bool ExistsSameCode { get; private set; }

        /// <summary>
        /// 表示同一个应用是否存在相同的用户名称
        /// </summary>
        public bool ExistsSameName { get; private set; }

        public ValidateApiPermissionGroupExistsSameNameEventResult(bool existsSameCode, bool existsSameName)
        {
            this.ExistsSameCode = existsSameCode;
            this.ExistsSameName = existsSameName;
        }
    }
}
