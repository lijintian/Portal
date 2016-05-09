

using System;
using System.Linq;
using Portal.Infrastructure.Exceptions;
using EasyDDD.Core.Specification;

namespace Portal.Domain.Specification.Permission
{
    public class PermissionCodeListSpecification : Specification<Aggregates.PermissionAgg.Permission>
    {
        private readonly string[] _codes;
        public PermissionCodeListSpecification(string[] codes)
        {
            CheckArgument.IsNotNullOrEmpty(codes, "codes");
            _codes = codes;
        }

        public override System.Linq.Expressions.Expression<Func<Aggregates.PermissionAgg.Permission, bool>> GetExpression()
        {
            return c => _codes.Contains(c.Code);
        }
    }
}
