
using System;

using EasyDDD.Infrastructure.Data.MongoDB;
using EasyDDD.Core.Repository;
using Portal.Domain.Aggregates.PermissionAgg;
using Portal.Domain.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;


using Portal.Domain.Specification.Permission;
using MongoDB.Bson;
using MongoDB.Driver.Builders;
using EasyDDD.Core.Specification;
using EasyDDD.Infrastructure.Crosscutting.Paged;
using EasyDDD.Core.Aggregate;

namespace Portal.Infrastructure.Data.MongoDB.Repository
{
    public class PermissionRepository : BaseRepository<Permission>, IPermissionRepository
    {
        public PermissionRepository(IRepositoryContext context)
            : base(context, "Permission")
        {
        }

        public PagePermission GetPagePermission(string code)
        {
            var permission = FindOne<PagePermission>(Query.And(Query.EQ("_t", "PagePermission"), Query.EQ("Code", code)));
            return permission;
        }


        public FunctionPermission GetFunctionPermission(string code)
        {
            var permission = FindOne<FunctionPermission>(Query.And(Query.EQ("_t", "FunctionPermission"), Query.EQ("Code", code)));
            return permission;
        }

        public ApiPermission GetApiPermission(string code)
        {
            var permission = FindOne<ApiPermission>(Query.And(Query.EQ("_t", "ApiPermission"), Query.EQ("Code", code)));
            return permission;
        }

        public IEnumerable<PagePermission> GetPagePermissionList(ISpecification<PagePermission> spceification)
        {
            return GetPermissionList("PagePermission", spceification);
        }

        public IEnumerable<ApiPermission> GetApiPermissionList(ISpecification<ApiPermission> spceification)
        {
            return GetPermissionList("ApiPermission", spceification);
        }

        public IEnumerable<FunctionPermission> GetFunctionPermission(string[] codes)
        {
            var permissions = Find<FunctionPermission>(Query.And(Query.EQ("_t", "FunctionPermission"), Query.In("Code", new BsonArray(codes)))).OrderBy(u => u.Order);
            return permissions;
        }

        public IEnumerable<PagePermission> GetPagePermissionList()
        {
            var permissions = Find<PagePermission>(Query.And(Query.EQ("_t", "PagePermission")));
            return permissions;
        }

        public IEnumerable<PagePermission> GetPagePermissionList(string appId)
        {
            var permissions = Find<PagePermission>(Query.And(Query.EQ("_t", "PagePermission"), Query.EQ("ApplicationId", appId)));
            return permissions;
        }

        public PagedResult<PagePermission> FindPagePermissionsInPage(
           int pageNumber,
           int pageSize,
           ISpecification<PagePermission> spceification,
           Dictionary<Expression<Func<PagePermission, dynamic>>, SortOrder> orderBys)
        {
            IQueryable<PagePermission> list = Find(Query.EQ("_t", "PagePermission"), spceification);
            return FindInPage(pageNumber, pageSize, list, orderBys);
        }

        public PagedResult<ApiPermission> FindApiPermissionsInPage(
           int pageNumber,
           int pageSize,
           ISpecification<ApiPermission> spceification,
           Dictionary<Expression<Func<ApiPermission, dynamic>>, SortOrder> orderBys)
        {
            IQueryable<ApiPermission> list = Find(Query.EQ("_t", "ApiPermission"), spceification);
            return FindInPage(pageNumber, pageSize, list, orderBys);
        }

        /// <summary>
        /// 根据类型和条件判断是否存在记录
        /// </summary>
        /// <param name="isApi"></param>
        /// <param name="spceification"></param>
        /// <returns></returns>
        public bool Exists(bool isApi, ISpecification<Permission> spceification)
        {
            string type = isApi ? "ApiPermission" : "PagePermission";
            return Find(Query.EQ("_t", type), spceification).Any();
        }

        #region 私有方法
        private IQueryable<T> GetPermissionList<T>(string typeName, ISpecification<T> spceification) where T : class, IAggregateRoot
        {
            return Find(Query.EQ("_t", typeName), spceification);
        }
        #endregion


        public IEnumerable<ApiPermission> GetApiPermssinsByCodeList(string[] codes)
        {
            if (codes == null || codes.Length == 0)
            {
                return Enumerable.Empty<ApiPermission>();
            }
            var permissions = this.GetList(new PermissionCodeListSpecification(codes));
            return Array.ConvertAll(permissions.ToArray(), item => (ApiPermission) item);
        }
    }
}
