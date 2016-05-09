
using EasyDDD.Core.Specification;
using System;

namespace Portal.Domain.Specification.ApiPermissionGroup
{
    public class ApiPermissionGroupCodeSpecification : Specification<Aggregates.ApiPermissionGroupAgg.ApiPermissionGroup>
    {
        private readonly string _code;
        public ApiPermissionGroupCodeSpecification(string code)
        {
            _code = code;
        }

        public override System.Linq.Expressions.Expression<Func<Aggregates.ApiPermissionGroupAgg.ApiPermissionGroup, bool>> GetExpression()
        {
            return c => c.Code == _code;
        }
    }
}
