using System;
using EasyDDD.Core.Specification;

namespace Portal.Domain.Specification.DeveloperApp
{
    public class DeveloperAppIsDeletedSpecification : Specification<Aggregates.DeveloperAppAgg.DeveloperApp>
    {
        private readonly bool _isDeleted;
        public DeveloperAppIsDeletedSpecification(bool isDeleted)
        {
            this._isDeleted = isDeleted;
        }

        public override System.Linq.Expressions.Expression<Func<Aggregates.DeveloperAppAgg.DeveloperApp, bool>> GetExpression()
        {
            return item => item.IsDeleted == _isDeleted;
        }
    }
}
