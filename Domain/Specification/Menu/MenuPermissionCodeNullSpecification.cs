using EasyDDD.Core.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Portal.Domain.Specification.Menu
{
    public class MenuPermissionCodeNullSpecification : Specification<Aggregates.MenuAgg.Menu>
    {
        public MenuPermissionCodeNullSpecification()
        {

        }

        public override System.Linq.Expressions.Expression<Func<Aggregates.MenuAgg.Menu, bool>> GetExpression()
        {
            return item => string.IsNullOrEmpty(item.PermissionCode);
        }
    }
}
