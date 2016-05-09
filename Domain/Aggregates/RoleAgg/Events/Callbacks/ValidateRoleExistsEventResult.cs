

using EasyDDD.Core.Event;

namespace Portal.Domain.Aggregates.RoleAgg.Events.Callbacks
{
    public class ValidateRoleExistsEventResult : IDomainEventResult
    {
        /// <summary>
        /// 系统不存在的角色Codes
        /// </summary>
        public string[] NoFoundRoleCodes { get; private set; }


        public ValidateRoleExistsEventResult(string[] noFoundRoleCodes)
        {
            this.NoFoundRoleCodes = noFoundRoleCodes;
        }
    }
}
