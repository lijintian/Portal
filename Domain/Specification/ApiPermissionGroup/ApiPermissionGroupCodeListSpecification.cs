
using EasyDDD.Core.Specification;
using System;
using System.Linq;

namespace Portal.Domain.Specification.ApiPermissionGroup
{
    public class ApiPermissionGroupCodeListSpecification : Specification<Aggregates.ApiPermissionGroupAgg.ApiPermissionGroup>
    {
        private readonly string[] _codes;
        public ApiPermissionGroupCodeListSpecification(string[] codes)
        {
            _codes = codes;
        }

        public override System.Linq.Expressions.Expression<Func<Aggregates.ApiPermissionGroupAgg.ApiPermissionGroup, bool>> GetExpression()
        {
            return c => _codes.Contains(c.Code);
        }
    }
}
