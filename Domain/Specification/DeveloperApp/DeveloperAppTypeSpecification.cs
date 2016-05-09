using System;
using EasyDDD.Core.Specification;
using Portal.Domain.Aggregates.DeveloperAppAgg;

namespace Portal.Domain.Specification.DeveloperApp
{
    public class DeveloperAppTypeSpecification : Specification<Aggregates.DeveloperAppAgg.DeveloperApp>
    {
        private readonly ApplicationType _type;
        public DeveloperAppTypeSpecification(int type)
        {
            this._type = (ApplicationType)type;
        }

        public override System.Linq.Expressions.Expression<Func<Aggregates.DeveloperAppAgg.DeveloperApp, bool>> GetExpression()
        {
            return item => item.AppType == _type;
        }
    }
}
