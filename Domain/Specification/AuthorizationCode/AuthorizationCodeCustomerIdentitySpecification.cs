using System;
using EasyDDD.Core.Specification;


namespace Portal.Domain.Specification.AuthorizationCode
{
    public class AuthorizationCodeCustomerIdentitySpecification : Specification<Aggregates.AuthorizationCodeAgg.AuthorizationCode>
    {
        private readonly string _customerIdentity;
        public AuthorizationCodeCustomerIdentitySpecification(string userId)
        {
            this._customerIdentity = userId;
        }

        public override System.Linq.Expressions.Expression<Func<Aggregates.AuthorizationCodeAgg.AuthorizationCode, bool>> GetExpression()
        {
            return item => item.CustomerIdentity == this._customerIdentity;
        }
    }
}
