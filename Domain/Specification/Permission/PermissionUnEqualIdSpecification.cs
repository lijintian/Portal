using EasyDDD.Core.Specification;
using System;


namespace Portal.Domain.Specification.Permission
{
    public class PermissionUnEqualIdSpecification : Specification<Aggregates.PermissionAgg.Permission>
    {
        public string Id { get; private set; }
        public PermissionUnEqualIdSpecification(string id)
        {
            this.Id = id;
        }
        public override System.Linq.Expressions.Expression<Func<Aggregates.PermissionAgg.Permission, bool>> GetExpression()
        {
            return p => p.Id != this.Id;
        }
    }
}
