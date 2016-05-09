

using EasyDDD.Core.Event;

namespace Portal.Domain.Aggregates.UserAgg.Events
{
    public class ResetedPasswordEvent : DomainEvent
    {
        public string NewPassword { get; private set; }
        public string UserEmail { get; set; }
        public ResetedPasswordEvent(User user, string newPassword,string userEmail)
            : base(user)
        {
            this.NewPassword = newPassword;
            this.UserEmail = userEmail;
        }
    }
}
