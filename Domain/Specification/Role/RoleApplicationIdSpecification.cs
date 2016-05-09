
using EasyDDD.Core.Specification;
using System;

namespace Portal.Domain.Specification.Role
{
    public class RoleApplicationIdSpecification : Specification<Aggregates.RoleAgg.Role>
    {
        private readonly string _applicationId;
        public RoleApplicationIdSpecification(string applicationId)
        {
            _applicationId = applicationId;
        }
        public override System.Linq.Expressions.Expression<Func<Aggregates.RoleAgg.Role, bool>> GetExpression()
        {
            return c => c.ApplicationId == _applicationId;
        }
    }
}
