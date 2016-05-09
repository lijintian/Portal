
using System;
using EasyDDD.Core.Specification;
namespace Portal.Domain.Specification.User
{
    public class UserLoginNameSpecification : Specification<Aggregates.UserAgg.User>
    {
        private readonly string _loginName;
        public UserLoginNameSpecification(string loginName)
        {
            _loginName = loginName.ToLower();
        }

        public override System.Linq.Expressions.Expression<Func<Aggregates.UserAgg.User, bool>> GetExpression()
        {
            return c => c.LoginName.ToLower() == _loginName;
        }
    }
}
