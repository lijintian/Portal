using System;

using Portal.Domain.Aggregates.PermissionAgg;
using System.Collections.Generic;
using System.Linq.Expressions;
using EasyDDD.Core.Repository;
using EasyDDD.Core.Specification;
using EasyDDD.Infrastructure.Crosscutting.Paged;

namespace Portal.Domain.Repositories
{
    public interface IPermissionRepository : IRepository<Permission>
    {
        PagePermission GetPagePermission(string code);
        FunctionPermission GetFunctionPermission(string code);

        IEnumerable<FunctionPermission> GetFunctionPermission(string[] codes);

        ApiPermission GetApiPermission(string code);

        IEnumerable<ApiPermission> GetApiPermssinsByCodeList(string[] codes);

        IEnumerable<PagePermission> GetPagePermissionList();

        IEnumerable<PagePermission> GetPagePermissionList(ISpecification<PagePermission> spceification);

        IEnumerable<ApiPermission> GetApiPermissionList(ISpecification<ApiPermission> spceification);

        IEnumerable<PagePermission> GetPagePermissionList(string appId);

        PagedResult<PagePermission> FindPagePermissionsInPage(
            int pageNumber,
            int pageSize,
            ISpecification<PagePermission> spceification,
            Dictionary<Expression<Func<PagePermission, dynamic>>, SortOrder> orderBys);

        PagedResult<ApiPermission> FindApiPermissionsInPage(
            int pageNumber,
            int pageSize,
            ISpecification<ApiPermission> spceification,
            Dictionary<Expression<Func<ApiPermission, dynamic>>, SortOrder> orderBys);

        /// <summary>
        /// 根据类型和条件判断是否存在记录
        /// </summary>
        /// <param name="isApi"></param>
        /// <param name="spceification"></param>
        /// <returns></returns>
        bool Exists(bool isApi, ISpecification<Permission> spceification);
    }
}
