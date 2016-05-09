using EasyDDD.Core.Specification;
using System;

namespace Portal.Domain.Specification.ApiPermissionGroup
{
    public class ApiPermissionGroupNameSpecification : Specification<Aggregates.ApiPermissionGroupAgg.ApiPermissionGroup>
    {
        private readonly string _name;
        public ApiPermissionGroupNameSpecification(string name)
        {
            _name = name;
        }

        public override System.Linq.Expressions.Expression<Func<Aggregates.ApiPermissionGroupAgg.ApiPermissionGroup, bool>> GetExpression()
        {
            return c => c.Name == _name;
        }
    }
}
