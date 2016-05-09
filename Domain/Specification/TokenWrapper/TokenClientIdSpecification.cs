using System;
using EasyDDD.Core.Specification;

namespace Portal.Domain.Specification.TokenWrapper
{
    public class TokenClientIdSpecification: Specification<Aggregates.TokenWrapperAgg.TokenWrapper>
    {
        private readonly string _clientId;
        public TokenClientIdSpecification(string clientId)
        {
            this._clientId = clientId;
        }
        public override System.Linq.Expressions.Expression<Func<Aggregates.TokenWrapperAgg.TokenWrapper, bool>> GetExpression()
        {
            return item => item.ClientId == this._clientId;
        }
    }
}
