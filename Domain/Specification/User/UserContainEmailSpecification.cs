using System;
using EasyDDD.Core.Specification;

namespace Portal.Domain.Specification.User
{
    public class UserContainEmailSpecification: Specification<Aggregates.UserAgg.User>
    {
         private readonly string _email;
         public UserContainEmailSpecification(string email)
        {
            _email = email.ToLower();
        }

        public override System.Linq.Expressions.Expression<Func<Aggregates.UserAgg.User, bool>> GetExpression()
        {
            return u => u.Email.ToLower().Contains(this._email);
        }
    }
}
