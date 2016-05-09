using System;
using System.Linq;
using EasyDDD.Core.Specification;

namespace Portal.Domain.Specification.ApiPermissionGroup
{
    public class ApiPermissionGroupExistsPermissionSpecification: Specification<Aggregates.ApiPermissionGroupAgg.ApiPermissionGroup>
    {
        public ApiPermissionGroupExistsPermissionSpecification()
        {
        }

        public override System.Linq.Expressions.Expression<Func<Aggregates.ApiPermissionGroupAgg.ApiPermissionGroup, bool>> GetExpression()
        {
            return c => c.Permissions!=null && c.Permissions.Any();
        }
    }
}
