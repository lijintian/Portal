

using System;
using Portal.Infrastructure.Exceptions;
using EasyDDD.Core.Specification;

namespace Portal.Domain.Specification.Permission
{
    public class ApiPermissionCodeSpecification : Specification<Aggregates.PermissionAgg.ApiPermission>
    {
        private readonly string _code;
        public ApiPermissionCodeSpecification(string code)
        {
            CheckArgument.IsNotNullOrEmpty(code, "code");
            _code = code;
        }

        public override System.Linq.Expressions.Expression<Func<Aggregates.PermissionAgg.ApiPermission, bool>> GetExpression()
        {
            return c => c.Code.ToLower().Contains(_code.ToLower());
        }
    }
}
