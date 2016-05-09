using System;
using EasyDDD.Core.Specification;

namespace Portal.Domain.Specification.AuthorizationCode
{
    public class AuthorizationCodeAuthorizationTimeEndSpecification: Specification<Aggregates.AuthorizationCodeAgg.AuthorizationCode>
    {
        public DateTime End { get; private set; }

        public AuthorizationCodeAuthorizationTimeEndSpecification(DateTime end)
        {
            this.End = end.Date.AddDays(1);
        }

        public override System.Linq.Expressions.Expression<Func<Aggregates.AuthorizationCodeAgg.AuthorizationCode, bool>> GetExpression()
        {
            return item => item.AuthorizationTime < this.End;
        }
    }
}
