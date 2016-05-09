

using System;
using Portal.Infrastructure.Exceptions;
using EasyDDD.Core.Specification;

namespace Portal.Domain.Specification.Permission
{
    public class PermissionCodeSpecification : Specification<Aggregates.PermissionAgg.Permission>
    {
        private readonly string _code;
        public PermissionCodeSpecification(string code)
        {
            CheckArgument.IsNotNullOrEmpty(code, "code");
            _code = code;
        }

        public override System.Linq.Expressions.Expression<Func<Aggregates.PermissionAgg.Permission, bool>> GetExpression()
        {
            return c => c.Code == _code;
        }
    }
}
