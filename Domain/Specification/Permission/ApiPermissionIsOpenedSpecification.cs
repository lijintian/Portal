using EasyDDD.Core.Specification;
using System;


namespace Portal.Domain.Specification.Permission
{
    public class ApiPermissionIsOpenedSpecification : Specification<Aggregates.PermissionAgg.ApiPermission>
    {
        public bool IsOpened { get; private set; }

        #region 初始化
        public ApiPermissionIsOpenedSpecification(bool isOpened)
        {
            this.IsOpened = isOpened;
        }
        #endregion

        public override System.Linq.Expressions.Expression<Func<Aggregates.PermissionAgg.ApiPermission, bool>> GetExpression()
        {
            return item => item.IsOpened == this.IsOpened;
        }
    }
}
