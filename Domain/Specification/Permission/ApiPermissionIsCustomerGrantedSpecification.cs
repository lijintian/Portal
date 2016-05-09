using EasyDDD.Core.Specification;
using System;


namespace Portal.Domain.Specification.Permission
{
    public class ApiPermissionIsCustomerGrantedSpecification: Specification<Aggregates.PermissionAgg.ApiPermission>
    {
        public bool IsCustomerGranted { get; private set; }

        #region 初始化
        public ApiPermissionIsCustomerGrantedSpecification(bool isCustomerGranted)
        {
            this.IsCustomerGranted = isCustomerGranted;
        }
        #endregion

        public override System.Linq.Expressions.Expression<Func<Aggregates.PermissionAgg.ApiPermission, bool>> GetExpression()
        {
            return item => item.IsCustomerGranted == this.IsCustomerGranted;
        }
    }
}