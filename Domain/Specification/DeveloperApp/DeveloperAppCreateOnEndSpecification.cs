using System;
using EasyDDD.Core.Specification;

namespace Portal.Domain.Specification.DeveloperApp
{
    public class DeveloperAppCreateOnEndSpecification : Specification<Aggregates.DeveloperAppAgg.DeveloperApp>
    {
        public DateTime End { get; private set; }

        public DeveloperAppCreateOnEndSpecification(DateTime end)
        {
            this.End = end.Date.AddDays(1);
        }

        public override System.Linq.Expressions.Expression<Func<Aggregates.DeveloperAppAgg.DeveloperApp, bool>> GetExpression()
        {
            return item => item.CreatedOn < this.End;
        }
    }
}
