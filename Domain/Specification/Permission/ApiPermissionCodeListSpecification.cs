using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using Portal.Infrastructure.Exceptions;
using EasyDDD.Core.Specification;

namespace Portal.Domain.Specification.Permission
{
    /// <summary>
    /// 表示API权限列表查询
    /// </summary>
    public class ApiPermissionCodeListSpecification : Specification<Aggregates.PermissionAgg.ApiPermission>
    {
         private readonly string[] _codes;
         public ApiPermissionCodeListSpecification(string[] codes)
        {
            CheckArgument.IsNotNullOrEmpty(codes, "codes");
            _codes = codes;
        }

         public override System.Linq.Expressions.Expression<Func<Aggregates.PermissionAgg.ApiPermission, bool>> GetExpression()
         {
             return item => this._codes.Contains(item.Code);
         }
    }
}
