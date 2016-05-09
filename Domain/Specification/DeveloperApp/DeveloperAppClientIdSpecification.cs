using System;
using EasyDDD.Core.Specification;

namespace Portal.Domain.Specification.DeveloperApp
{
    /// <summary>
    /// 表示客户Id规格
    /// </summary>
    public class DeveloperAppClientIdSpecification : Specification<Aggregates.DeveloperAppAgg.DeveloperApp>
    {
        private readonly string _clientId;
        public DeveloperAppClientIdSpecification(string clientId)
        {
            this._clientId = clientId;
        }

        public override System.Linq.Expressions.Expression<Func<Aggregates.DeveloperAppAgg.DeveloperApp, bool>> GetExpression()
        {
            return item => item.ClientId == _clientId;
        }
    }
}
