
using EasyDDD.Core.Specification;
using System;
using System.Linq;

namespace Portal.Domain.Specification.Role
{
    public class RoleCodeListSpecification : Specification<Aggregates.RoleAgg.Role>
    {
        private readonly string[] _codes;
        public RoleCodeListSpecification(string[] codes)
        {
            _codes = codes;
        }

        public override System.Linq.Expressions.Expression<Func<Aggregates.RoleAgg.Role, bool>> GetExpression()
        {
            return c => _codes.Contains(c.Code);
        }
    }
}
