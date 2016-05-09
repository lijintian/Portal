using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using EasyDDD.Core.Specification;
namespace Portal.Domain.Specification.User
{
    public class UserOwnedRoleSpecification : Specification<Aggregates.UserAgg.User>
    {
        private string Rolecode { get; set; }
        public UserOwnedRoleSpecification(string roleCode)
        {
            this.Rolecode = roleCode;
        }
        public override System.Linq.Expressions.Expression<Func<Aggregates.UserAgg.User, bool>> GetExpression()
        {
            return user => user.Roles.Contains(this.Rolecode);
        }
    }
}
