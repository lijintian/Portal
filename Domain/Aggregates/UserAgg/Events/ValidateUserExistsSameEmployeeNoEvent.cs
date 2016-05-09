using EasyDDD.Core.Event;

namespace Portal.Domain.Aggregates.UserAgg.Events
{
    public class ValidateUserExistsSameEmployeeNoEvent : DomainEvent
    {
        public string EmployeeNo { get; private set; }

        public ValidateUserExistsSameEmployeeNoEvent(User user, string employeeNo)
            : base(user)
        {
            this.EmployeeNo = employeeNo;
        }
    }
}
