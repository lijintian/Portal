using System;
using EasyDDD.Core.Specification;

namespace Portal.Domain.Specification.TokenWrapper
{
    public class TokenCreateOnEndSpecification : Specification<Aggregates.TokenWrapperAgg.TokenWrapper>
    {
        public DateTime End { get; private set; }

        public TokenCreateOnEndSpecification(DateTime end)
        {
            this.End = end.Date.AddDays(1);
        }

        public override System.Linq.Expressions.Expression<Func<Aggregates.TokenWrapperAgg.TokenWrapper, bool>> GetExpression()
        {
            return item => item.CreatedOn < this.End;
        }
    }
}
