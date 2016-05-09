
using EasyDDD.Core.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Domain.Specification.Permission
{
    public class PermissionEqualNameSpecification : Specification<Aggregates.PermissionAgg.Permission>
    {
        public string Name { get; private set; }
        public PermissionEqualNameSpecification(string name)
        {
            this.Name = name;
        }
        public override System.Linq.Expressions.Expression<Func<Aggregates.PermissionAgg.Permission, bool>> GetExpression()
        {
            return p => p.Name == this.Name;
        }
    }
}
