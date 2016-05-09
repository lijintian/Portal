using System;

using Portal.Domain.Aggregates.UserAgg;
using EasyDDD.Core.Specification;
namespace Portal.Domain.Specification.User
{
    public class UserTypeSpecification : Specification<Aggregates.UserAgg.User>
    {
        private readonly UserType _type;
        public UserTypeSpecification(UserType type)
        {
            this._type = type;
        }
        public override System.Linq.Expressions.Expression<Func<Aggregates.UserAgg.User, bool>> GetExpression()
        {
            return item => item.UserType == this._type;
        }
    }
}
