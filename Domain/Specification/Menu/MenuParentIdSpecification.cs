using EasyDDD.Core.Specification;
using System;
using System.Linq;


namespace Portal.Domain.Specification.Menu
{
    public class MenuParentIdSpecification : Specification<Aggregates.MenuAgg.Menu>
    {
        /// <summary>
        /// 父节点ID
        /// </summary>
        public string ParentId { get; set; }

        public MenuParentIdSpecification(string parentid)
        {
            this.ParentId = parentid;
        }

        public override System.Linq.Expressions.Expression<Func<Aggregates.MenuAgg.Menu, bool>> GetExpression()
        {
            return item => ParentId == item.ParentId;
        }
    }
}
