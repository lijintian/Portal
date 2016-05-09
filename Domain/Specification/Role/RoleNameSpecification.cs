
using EasyDDD.Core.Specification;
using System;

namespace Portal.Domain.Specification.Role
{
    public class RoleNameSpecification : Specification<Aggregates.RoleAgg.Role>
    {
        private readonly string _name;
        public RoleNameSpecification(string name)
        {
            _name = name;
        }

        public override System.Linq.Expressions.Expression<Func<Aggregates.RoleAgg.Role, bool>> GetExpression()
        {
            return c => c.Name == _name;
        }
    }
}
