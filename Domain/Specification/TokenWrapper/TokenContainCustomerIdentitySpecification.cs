using System;


using EasyDDD.Core.Specification;
namespace Portal.Domain.Specification.TokenWrapper
{
    public class TokenContainCustomerIdentitySpecification : Specification<Aggregates.TokenWrapperAgg.TokenWrapper>
    {
        private readonly string _customerIdentity;
        public TokenContainCustomerIdentitySpecification(string userId)
        {
            this._customerIdentity = userId.ToLower();
        }
        public override System.Linq.Expressions.Expression<Func<Aggregates.TokenWrapperAgg.TokenWrapper, bool>> GetExpression()
        {
            return item => item.CustomerIdentity.ToLower().Contains(this._customerIdentity);
        }
    }
}
