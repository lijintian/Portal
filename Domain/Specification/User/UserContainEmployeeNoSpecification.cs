using System;
using EasyDDD.Core.Specification;


namespace Portal.Domain.Specification.User
{
    public class UserContainEmployeeNoSpecification: Specification<Aggregates.UserAgg.User>
    {
        private readonly string _employeeNo;
        public UserContainEmployeeNoSpecification(string employeeNo)
        {
            _employeeNo = employeeNo.ToLower();
        }

        public override System.Linq.Expressions.Expression<Func<Aggregates.UserAgg.User, bool>> GetExpression()
        {
            return u => u.EmployeeNo.ToLower().Contains(this._employeeNo);
        }
    }
}
