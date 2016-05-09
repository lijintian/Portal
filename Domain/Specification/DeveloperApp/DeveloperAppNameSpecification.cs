using System;
using EasyDDD.Core.Specification;

namespace Portal.Domain.Specification.DeveloperApp
{
    public class DeveloperAppNameSpecification : Specification<Aggregates.DeveloperAppAgg.DeveloperApp>
    {
        private readonly string _name;
        public DeveloperAppNameSpecification(string name)
        {
            this._name = name.ToLower();
        }

        public override System.Linq.Expressions.Expression<Func<Aggregates.DeveloperAppAgg.DeveloperApp, bool>> GetExpression()
        {
            return item => item.Name.ToLower() == _name;
        }
    }
}
