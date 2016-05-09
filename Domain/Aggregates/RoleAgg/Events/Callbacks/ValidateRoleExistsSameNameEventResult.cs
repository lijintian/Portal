

using EasyDDD.Core.Event;

namespace Portal.Domain.Aggregates.RoleAgg.Events.Callbacks
{
    public class ValidateRoleExistsSameNameEventResult : IDomainEventResult
    {
        /// <summary>
        /// 表示是否存在相同的角色Code
        /// </summary>
        public bool ExistsSameCode { get; private set; }

        /// <summary>
        /// 表示同一个应用是否存在相同的用户名称
        /// </summary>
        public bool ExistsSameName { get; private set; }

        public ValidateRoleExistsSameNameEventResult(bool existsSameCode, bool existsSameName)
        {
            this.ExistsSameCode = existsSameCode;
            this.ExistsSameName = existsSameName;
        }
    }
}
