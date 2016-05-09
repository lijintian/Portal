using System;
using EasyDDD.Core.Specification;

namespace Portal.Domain.Specification.User
{
    public class UserContainPhoneSpecification : Specification<Aggregates.UserAgg.User>
    {
        private readonly string _phone;
        public UserContainPhoneSpecification(string phone)
        {
            _phone = phone.ToLower();
        }

        public override System.Linq.Expressions.Expression<Func<Aggregates.UserAgg.User, bool>> GetExpression()
        {
            return u => u.Phone.ToLower().Contains(this._phone);
        }
    }
}
