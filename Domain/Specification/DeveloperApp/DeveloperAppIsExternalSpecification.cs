using System;
using EasyDDD.Core.Specification;

namespace Portal.Domain.Specification.DeveloperApp
{
    public class DeveloperAppIsExternalSpecification : Specification<Aggregates.DeveloperAppAgg.DeveloperApp>
    {
        private readonly bool _isExternal;
        public DeveloperAppIsExternalSpecification(bool isExternal)
        {
            this._isExternal = isExternal;
        }

        public override System.Linq.Expressions.Expression<Func<Aggregates.DeveloperAppAgg.DeveloperApp, bool>> GetExpression()
        {
            return item => item.IsExternal == _isExternal;
        }
    }
}
