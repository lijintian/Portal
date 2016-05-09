using System.Linq;

using System;
using EasyDDD.Core.Specification;
namespace Portal.Domain.Specification.User
{
    public class EmployeeNoListSpecification : Specification<Aggregates.UserAgg.User>
    {
        private readonly string[] _employeeNos;
        public EmployeeNoListSpecification(string[] employeeNos)
        {
            _employeeNos = employeeNos;
        }

        public override System.Linq.Expressions.Expression<Func<Aggregates.UserAgg.User, bool>> GetExpression()
        {
            return u => this._employeeNos.Contains(u.EmployeeNo);
        }
    }
}
