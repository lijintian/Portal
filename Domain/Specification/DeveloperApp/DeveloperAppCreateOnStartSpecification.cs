using System;
using EasyDDD.Core.Specification;

namespace Portal.Domain.Specification.DeveloperApp
{
    public class DeveloperAppCreateOnStartSpecification : Specification<Aggregates.DeveloperAppAgg.DeveloperApp>
    {
        public DateTime Start { get; private set; }

        public DeveloperAppCreateOnStartSpecification(DateTime start)
        {
            this.Start = start.Date;
        }

        public override System.Linq.Expressions.Expression<Func<Aggregates.DeveloperAppAgg.DeveloperApp, bool>> GetExpression()
        {
            return item => item.CreatedOn >= this.Start;
        }
    }
}
