using System;
using EasyDDD.Core.Specification;
using Portal.Domain.Aggregates.DeveloperAppAgg;

namespace Portal.Domain.Specification.DeveloperApp
{
    public class DeveloperAppStateSpecification : Specification<Aggregates.DeveloperAppAgg.DeveloperApp>
    {
        private readonly AppState _state;
        public DeveloperAppStateSpecification(AppState state)
        {
            this._state = state;
        }
        public DeveloperAppStateSpecification(int state)
        {
            this._state = (AppState)state;
        }

        public override System.Linq.Expressions.Expression<Func<Aggregates.DeveloperAppAgg.DeveloperApp, bool>> GetExpression()
        {
            return item => item.State == _state;
        }
    }
}
