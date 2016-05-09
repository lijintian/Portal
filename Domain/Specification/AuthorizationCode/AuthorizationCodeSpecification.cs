using System;
using EasyDDD.Core.Specification;

namespace Portal.Domain.Specification.AuthorizationCode
{
    public class AuthorizationCodeSpecification : Specification<Aggregates.AuthorizationCodeAgg.AuthorizationCode>
    {
        private readonly string _code;
        public AuthorizationCodeSpecification(string code)
        {
            this._code = code;
        }

        public override System.Linq.Expressions.Expression<Func<Aggregates.AuthorizationCodeAgg.AuthorizationCode, bool>> GetExpression()
        {
            return item => item.Code == this._code;
        }
    }
}
