using System;
using EasyDDD.Core.Specification;

namespace Portal.Domain.Specification.ApiPermissionGroup
{
    public class ApiPermissionGroupContainCodeSpecification : Specification<Aggregates.ApiPermissionGroupAgg.ApiPermissionGroup>
    {
        private readonly string _code;
        public ApiPermissionGroupContainCodeSpecification(string code)
        {
            _code = code.ToLower();
        }

        public override System.Linq.Expressions.Expression<Func<Aggregates.ApiPermissionGroupAgg.ApiPermissionGroup, bool>> GetExpression()
        {
            return c => c.Code.ToLower().Contains(_code);
        }
    }
}
