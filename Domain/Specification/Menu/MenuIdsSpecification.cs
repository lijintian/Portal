using EasyDDD.Core.Specification;
using System;
using System.Linq;


namespace Portal.Domain.Specification.Menu
{
    public class MenuIdsSpecification : Specification<Aggregates.MenuAgg.Menu>
    {
        /// <summary>
        /// 多个ID
        /// </summary>
        public string[] Ids { get; set; }

        public MenuIdsSpecification(string[] ids)
        {
            this.Ids = ids;
        }

        public override System.Linq.Expressions.Expression<Func<Aggregates.MenuAgg.Menu, bool>> GetExpression()
        {
            return item => Ids.Contains(item.Id);
        }
    }
}
