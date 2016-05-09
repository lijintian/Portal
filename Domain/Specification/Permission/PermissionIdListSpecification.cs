

using System;
using System.Linq;
using Portal.Infrastructure.Exceptions;
using EasyDDD.Core.Specification;

namespace Portal.Domain.Specification.Permission
{
    public class PermissionIdListSpecification: Specification<Aggregates.PermissionAgg.Permission>
    {
        private readonly string[] _ids;
        public PermissionIdListSpecification(string[] ids)
        {
            CheckArgument.IsNotNullOrEmpty(ids, "ids");
            _ids = ids;
        }

        public override System.Linq.Expressions.Expression<Func<Aggregates.PermissionAgg.Permission, bool>> GetExpression()
        {
            return c => _ids.Contains(c.Id);
        }
    }
}
