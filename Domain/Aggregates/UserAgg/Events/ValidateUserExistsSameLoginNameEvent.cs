using EasyDDD.Core.Event;

namespace Portal.Domain.Aggregates.UserAgg.Events
{
    public class ValidateUserExistsSameLoginNameEvent : DomainEvent
    {
        public string LoginName { get; private set; }

        public ValidateUserExistsSameLoginNameEvent(User user, string loginName)
            : base(user)
        {
            this.LoginName = loginName;
        }
    }
}
