using System;
using EasyDDD.Core.Specification;

namespace Portal.Domain.Specification.AuthorizationCode
{
    public class AuthorizationCodeContainCodeSpecification : Specification<Aggregates.AuthorizationCodeAgg.AuthorizationCode>
    {
        private readonly string _code;
        public AuthorizationCodeContainCodeSpecification(string code)
        {
            this._code = code.ToLower();
        }

        public override System.Linq.Expressions.Expression<Func<Aggregates.AuthorizationCodeAgg.AuthorizationCode, bool>> GetExpression()
        {
            return item => item.Code.ToLower().Contains(this._code);
        }
    }
}
