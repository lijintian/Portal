using EasyDDD.Core.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Domain.Specification.Permission
{
    public class PagePermissionNameSpecification : Specification<Aggregates.PermissionAgg.PagePermission>
    {
        public string Name { get; private set; }
        public PagePermissionNameSpecification(string name)
        {
            this.Name = name;
        }
        public override System.Linq.Expressions.Expression<Func<Aggregates.PermissionAgg.PagePermission, bool>> GetExpression()
        {
            return p => !string.IsNullOrEmpty(p.Name) && p.Name.ToLower().Contains(this.Name.ToLower());
        }
    }
}
