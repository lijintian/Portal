using System;
using System.Collections.Generic;
using EasyDDD.Core.Specification;


namespace Portal.Domain.Specification.TokenWrapper
{
    public class TokenCustomerIdentitySpecification : Specification<Aggregates.TokenWrapperAgg.TokenWrapper>
    {
        private readonly string _customerIdentity;
        public TokenCustomerIdentitySpecification(string userId)
        {
            this._customerIdentity =userId.ToLower();
        }
        public override System.Linq.Expressions.Expression<Func<Aggregates.TokenWrapperAgg.TokenWrapper, bool>> GetExpression()
        {
            return item => item.CustomerIdentity.ToLower() == this._customerIdentity;
        }
    }
}
