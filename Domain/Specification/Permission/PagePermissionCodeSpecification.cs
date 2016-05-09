using EasyDDD.Core.Specification;

using System;
using Portal.Infrastructure.Exceptions;

namespace Portal.Domain.Specification.Permission
{
    public class PagePermissionCodeSpecification : Specification<Aggregates.PermissionAgg.PagePermission>
    {
        private readonly string _code;
        public PagePermissionCodeSpecification(string code)
        {
            CheckArgument.IsNotNullOrEmpty(code, "code");
            _code = code;
        }

        public override System.Linq.Expressions.Expression<Func<Aggregates.PermissionAgg.PagePermission, bool>> GetExpression()
        {
            return c => !string.IsNullOrEmpty(c.Code) && c.Code.ToLower().Contains(_code.ToLower());
        }
    }
}
