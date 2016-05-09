using System.Linq;


using System;
using Portal.Infrastructure.Exceptions;
using EasyDDD.Core.Specification;

namespace Portal.Domain.Specification.Permission
{
    public class ApiPermissionContainCodeSpecification : Specification<Aggregates.PermissionAgg.ApiPermission>
    {
        private readonly string[] _code;

        public ApiPermissionContainCodeSpecification(string[] code)
        {
            CheckArgument.IsNotNullOrEmpty(code, "code");
            _code = code;
        }

        public override System.Linq.Expressions.Expression<Func<Aggregates.PermissionAgg.ApiPermission, bool>> GetExpression()
        {
            return c => this._code.Contains(c.Code, StringComparer.CurrentCultureIgnoreCase);
        }
    }
}
