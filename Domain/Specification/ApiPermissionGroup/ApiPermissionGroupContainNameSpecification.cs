using System;

using EasyDDD.Core.Specification;
namespace Portal.Domain.Specification.ApiPermissionGroup
{
    public class ApiPermissionGroupContainNameSpecification : Specification<Aggregates.ApiPermissionGroupAgg.ApiPermissionGroup>
    {
        private readonly string _name;
        public ApiPermissionGroupContainNameSpecification(string name)
        {
            _name = name.ToLower();
        }

        public override System.Linq.Expressions.Expression<Func<Aggregates.ApiPermissionGroupAgg.ApiPermissionGroup, bool>> GetExpression()
        {
            return c => c.Name.ToLower().Contains(_name);
        }
    }
}
