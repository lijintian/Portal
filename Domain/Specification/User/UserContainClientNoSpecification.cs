using System;

using EasyDDD.Core.Specification;
namespace Portal.Domain.Specification.User
{
    public class UserContainClientNoSpecification : Specification<Aggregates.UserAgg.User>
    {
        private readonly string _clientNo;
        public UserContainClientNoSpecification(string clientNo)
        {
            _clientNo = clientNo.ToLower();
        }
        public override System.Linq.Expressions.Expression<Func<Aggregates.UserAgg.User, bool>> GetExpression()
        {
            return u => u.ClientNo.ToLower().Contains(this._clientNo);
        }
    }
}
