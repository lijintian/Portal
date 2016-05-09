using System.Web.UI;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyDDD.Core.Specification;
namespace Portal.Domain.Specification.User
{
    public class CreatedOnStartSpecification : Specification<Aggregates.UserAgg.User>
    {
        public DateTime Start { get; private set; }

        public CreatedOnStartSpecification(DateTime start)
        {
            this.Start = start.Date;
        }

        public override System.Linq.Expressions.Expression<Func<Aggregates.UserAgg.User, bool>> GetExpression()
        {
            return item => item.CreatedOn >= this.Start;
        }
    }
}
