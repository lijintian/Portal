using System;
using EasyDDD.Core.Specification;

namespace Portal.Domain.Specification.AuthorizationCode
{
    public class AuthorizationCodeClientIdSpecification : Specification<Aggregates.AuthorizationCodeAgg.AuthorizationCode>
    {
        private readonly string _clientId;
        public AuthorizationCodeClientIdSpecification(string clientId)
        {
            this._clientId = clientId;
        }

        public override System.Linq.Expressions.Expression<Func<Aggregates.AuthorizationCodeAgg.AuthorizationCode, bool>> GetExpression()
        {
            return item => item.ClientId == this._clientId;
        }
    }
}
