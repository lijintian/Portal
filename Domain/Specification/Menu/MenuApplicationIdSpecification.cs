using EasyDDD.Core.Specification;
using System;
using System.Linq;


namespace Portal.Domain.Specification.Menu
{
    public class MenuApplicationIdSpecification : Specification<Aggregates.MenuAgg.Menu>
    {
        /// <summary>
        /// 多个ID
        /// </summary>
        private string appId { get; set; }

        public MenuApplicationIdSpecification(string appId)
        {
            this.appId = appId;
        }

        public override System.Linq.Expressions.Expression<Func<Aggregates.MenuAgg.Menu, bool>> GetExpression()
        {
            return item => item.ApplicationId == this.appId;
        }
    }
}
