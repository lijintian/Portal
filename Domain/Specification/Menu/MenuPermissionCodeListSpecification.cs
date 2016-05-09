using System.Web.UI;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyDDD.Core.Specification;

namespace Portal.Domain.Specification.Menu
{
    public class MenuPermissionCodeListSpecification : Specification<Aggregates.MenuAgg.Menu>
    {
        public IEnumerable<string> PermissionCodes { get; private set; } 
        public MenuPermissionCodeListSpecification(IEnumerable<string> permissionCodes)
        {
            this.PermissionCodes = permissionCodes;
        }

        public override System.Linq.Expressions.Expression<Func<Aggregates.MenuAgg.Menu, bool>> GetExpression()
        {
            return item => PermissionCodes.Contains(item.PermissionCode);
        }
    }
}
