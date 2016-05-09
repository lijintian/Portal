using System;
using EasyDDD.Core.Specification;

namespace Portal.Domain.Specification.DeveloperApp
{
    public class DeveloperAppContainUserIdSpecification : Specification<Aggregates.DeveloperAppAgg.DeveloperApp>
    {
        private readonly string _userId;
        public DeveloperAppContainUserIdSpecification(string userId)
        {
            this._userId = userId.ToLower();
        }

        public override System.Linq.Expressions.Expression<Func<Aggregates.DeveloperAppAgg.DeveloperApp, bool>> GetExpression()
        {
            return item => item.UserId.ToLower().Contains(_userId);
        }
    }
}
