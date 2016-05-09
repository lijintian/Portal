using EasyDDD.Core.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Portal.Domain.Specification.Permission
{
    public class PermissionApplicationIdSpecification : Specification<Aggregates.PermissionAgg.Permission>
    {
        public string ApplicationId { get; private set; }
        public PermissionApplicationIdSpecification(string appId)
        {
            this.ApplicationId = appId;
        }

        public override System.Linq.Expressions.Expression<Func<Aggregates.PermissionAgg.Permission, bool>> GetExpression()
        {
            return p => p.ApplicationId == this.ApplicationId;
        }
    }
}
