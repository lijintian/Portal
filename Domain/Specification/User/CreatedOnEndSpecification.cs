using System.Web.UI;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyDDD.Core.Specification;
namespace Portal.Domain.Specification.User
{
    public class CreatedOnEndSpecification : Specification<Aggregates.UserAgg.User>
    {
        public DateTime End { get; private set; }

        public CreatedOnEndSpecification(DateTime end)
        {
            this.End = end.Date.AddDays(1);
        }

        public override System.Linq.Expressions.Expression<Func<Aggregates.UserAgg.User, bool>> GetExpression()
        {
            return item => item.CreatedOn < this.End;
        }
    }
}
