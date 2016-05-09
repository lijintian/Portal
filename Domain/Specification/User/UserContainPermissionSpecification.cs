using System;
using System.Linq;
using EasyDDD.Core.Specification;

namespace Portal.Domain.Specification.User
{
    public class UserContainPermissionSpecification : Specification<Aggregates.UserAgg.User>
    {
        private readonly string _permissionCode;
        public UserContainPermissionSpecification(string permissionCode)
        {
            _permissionCode = permissionCode;
        }

        public override System.Linq.Expressions.Expression<Func<Aggregates.UserAgg.User, bool>> GetExpression()
        {
            return u => u.Permissions != null && u.Permissions.Contains(_permissionCode);
                //u.Permissions.Any(v => v.ToLower() == _permissionCode);
        }
    }
}
