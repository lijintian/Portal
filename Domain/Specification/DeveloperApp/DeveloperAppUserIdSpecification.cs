using System;
using EasyDDD.Core.Specification;

namespace Portal.Domain.Specification.DeveloperApp
{
    public class DeveloperAppUserIdSpecification : Specification<Aggregates.DeveloperAppAgg.DeveloperApp>
    {
        private readonly string _userId;
        public DeveloperAppUserIdSpecification(string userId)
        {
            this._userId = userId;
        }

        public override System.Linq.Expressions.Expression<Func<Aggregates.DeveloperAppAgg.DeveloperApp, bool>> GetExpression()
        {
            return item => item.UserId == _userId;
        }
    }
}
