using System;

using EasyDDD.Core.Specification;
namespace Portal.Domain.Specification.TokenWrapper
{
    public class AccessTokenSpecification : Specification<Aggregates.TokenWrapperAgg.TokenWrapper>
    {
        private readonly string _accessToken;
        public AccessTokenSpecification(string accessToken)
        {
            this._accessToken = accessToken;
        }
        public override System.Linq.Expressions.Expression<Func<Aggregates.TokenWrapperAgg.TokenWrapper, bool>> GetExpression()
        {
            return item => item.AccessToken == this._accessToken;
        }
    }
}
