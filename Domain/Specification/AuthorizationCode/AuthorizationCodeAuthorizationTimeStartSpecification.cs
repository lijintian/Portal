using System;
using EasyDDD.Core.Specification;

namespace Portal.Domain.Specification.AuthorizationCode
{
    public class AuthorizationCodeAuthorizationTimeStartSpecification : Specification<Aggregates.AuthorizationCodeAgg.AuthorizationCode>
    {
        public DateTime Start { get; private set; }

        public AuthorizationCodeAuthorizationTimeStartSpecification(DateTime start)
        {
            this.Start = start.Date;
        }

        public override System.Linq.Expressions.Expression<Func<Aggregates.AuthorizationCodeAgg.AuthorizationCode, bool>> GetExpression()
        {
            return item => item.AuthorizationTime >= this.Start;
        }
    }
}
