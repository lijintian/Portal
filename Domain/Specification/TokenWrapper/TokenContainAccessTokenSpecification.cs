using System;


using EasyDDD.Core.Specification;
namespace Portal.Domain.Specification.TokenWrapper
{
    public class TokenContainAccessTokenSpecification : Specification<Aggregates.TokenWrapperAgg.TokenWrapper>
    {
        private readonly string _accessToken;
        public TokenContainAccessTokenSpecification(string accessToken)
        {
            this._accessToken = accessToken.ToLower();
        }
        public override System.Linq.Expressions.Expression<Func<Aggregates.TokenWrapperAgg.TokenWrapper, bool>> GetExpression()
        {
            return item => item.AccessToken.ToLower().Contains(this._accessToken);
        }
    }
}
