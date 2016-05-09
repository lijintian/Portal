using EasyDDD.Core.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Portal.Domain.Specification.Permission
{
    public class PagePermissionApplicationIdSpecification : Specification<Aggregates.PermissionAgg.PagePermission>
    {
        public string ApplicationId { get; private set; }
        public PagePermissionApplicationIdSpecification(string appId)
        {
            this.ApplicationId = appId;
        }

        public override System.Linq.Expressions.Expression<Func<Aggregates.PermissionAgg.PagePermission, bool>> GetExpression()
        {
            return item => item.ApplicationId == this.ApplicationId;
        }
    }
}
