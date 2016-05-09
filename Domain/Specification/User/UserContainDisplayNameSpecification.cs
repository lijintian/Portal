using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using EasyDDD.Core.Specification;
namespace Portal.Domain.Specification.User
{
    public class UserContainDisplayNameSpecification : Specification<Aggregates.UserAgg.User>
    {
        private readonly string _displayName;
         public UserContainDisplayNameSpecification(string displayName)
        {
            _displayName = displayName;
        }

        public override System.Linq.Expressions.Expression<Func<Aggregates.UserAgg.User, bool>> GetExpression()
        {
            return u => u.DisplayName.ToLower().Contains(this._displayName.ToLower());
        }
    }
}
