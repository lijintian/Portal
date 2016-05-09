
using EasyDDD.Core.Specification;
using System;

namespace Portal.Domain.Specification.User
{
    public class EmployeeNoSpecification : Specification<Aggregates.UserAgg.User>
    {
        private readonly string _employeeNo;
        public EmployeeNoSpecification(string employeeNo)
        {
            _employeeNo = employeeNo;
        }

        public override System.Linq.Expressions.Expression<Func<Aggregates.UserAgg.User, bool>> GetExpression()
        {
            return c => c.EmployeeNo == _employeeNo;
        }
    }
}
