
using EasyDDD.Core.Specification;
using System;

namespace Portal.Domain.Specification.Role
{
    public class RoleCodeSpecification : Specification<Aggregates.RoleAgg.Role>
    {
        private readonly string _code;
        public RoleCodeSpecification(string code)
        {
            _code = code;
        }

        public override System.Linq.Expressions.Expression<Func<Aggregates.RoleAgg.Role, bool>> GetExpression()
        {
            return c => c.Code == _code;
        }
    }
}
