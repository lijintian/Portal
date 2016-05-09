using System;
using EasyDDD.Core.Specification;

namespace Portal.Domain.Specification.User
{
    public class UserContainLoginNameSpecification : Specification<Aggregates.UserAgg.User>
    {
         private readonly string _loginName;
        public UserContainLoginNameSpecification(string loginName)
        {
            _loginName = loginName;
        }

        public override System.Linq.Expressions.Expression<Func<Aggregates.UserAgg.User, bool>> GetExpression()
        {
            return u => u.LoginName.ToLower().Contains(this._loginName.ToLower());
        }
    }
}
