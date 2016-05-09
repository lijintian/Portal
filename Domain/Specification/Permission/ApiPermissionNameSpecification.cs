
using EasyDDD.Core.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Domain.Specification.Permission
{
    public class ApiPermissionNameSpecification : Specification<Aggregates.PermissionAgg.ApiPermission>
    {
        public string Name { get; private set; }
        public ApiPermissionNameSpecification(string name)
        {
            this.Name = name;
        }
        public override System.Linq.Expressions.Expression<Func<Aggregates.PermissionAgg.ApiPermission, bool>> GetExpression()
        {
            return p => p.Name.ToLower().Contains(this.Name.ToLower());
        }
    }
}
