using System;
using EasyDDD.Core.Specification;

namespace Portal.Domain.Specification.TokenWrapper
{
    public class TokenCreateOnStartSpecification : Specification<Aggregates.TokenWrapperAgg.TokenWrapper>
    {
        public DateTime Start { get; private set; }

        public TokenCreateOnStartSpecification(DateTime start)
        {
            this.Start = start.Date;
        }

        public override System.Linq.Expressions.Expression<Func<Aggregates.TokenWrapperAgg.TokenWrapper, bool>> GetExpression()
        {
            return item => item.CreatedOn >= this.Start;
        }
    }
}
