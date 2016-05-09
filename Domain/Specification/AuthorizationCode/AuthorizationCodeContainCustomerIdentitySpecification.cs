using System;
using EasyDDD.Core.Specification;

namespace Portal.Domain.Specification.AuthorizationCode
{
    public class AuthorizationCodeContainCustomerIdentitySpecification : Specification<Aggregates.AuthorizationCodeAgg.AuthorizationCode>
    {
        private readonly string _customerIdentity;
        public AuthorizationCodeContainCustomerIdentitySpecification(string userId)
        {
            this._customerIdentity = userId.ToLower();
        }

        public override System.Linq.Expressions.Expression<Func<Aggregates.AuthorizationCodeAgg.AuthorizationCode, bool>> GetExpression()
        {
            return item => item.CustomerIdentity.ToLower().Contains(this._customerIdentity);
        }
    }
}
