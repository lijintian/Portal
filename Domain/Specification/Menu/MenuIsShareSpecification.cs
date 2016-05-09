using EasyDDD.Core.Specification;
using System;
using System.Linq;


namespace Portal.Domain.Specification.Menu
{
    public class MenuIsShareSpecification : Specification<Aggregates.MenuAgg.Menu>
    {
        /// <summary>
        /// 多个ID
        /// </summary>
        public bool IsShare { get; set; }

        public MenuIsShareSpecification(bool isShare)
        {
            this.IsShare = isShare;
        }

        public override System.Linq.Expressions.Expression<Func<Aggregates.MenuAgg.Menu, bool>> GetExpression()
        {
            return item => item.IsShare == IsShare;
        }
    }
}
