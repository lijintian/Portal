

using EasyDDD.Core.Repository;
using EasyDDD.Infrastructure.Data.MongoDB;
using Portal.Domain.Aggregates.ApiPermissionGroupAgg;
using Portal.Domain.Repositories;

namespace Portal.Infrastructure.Data.MongoDB.Repository
{
    public class ApiPermissionGroupRepository : MongoDBRepository<ApiPermissionGroup>, IApiPermissionGroupRepository
    {
        public ApiPermissionGroupRepository(IRepositoryContext context)
            : base(context)
        {
        }
    }
}
