


using EasyDDD.Core.Repository;
using EasyDDD.Infrastructure.Data.MongoDB;
using Portal.Domain.Aggregates.ApiUserAgg;
using Portal.Domain.Repositories;

namespace Portal.Infrastructure.Data.MongoDB.Repository
{
    public class ApiUserRepository : MongoDBRepository<ApiUser>, IApiUserRepository
    {
        public ApiUserRepository(IRepositoryContext context)
            : base(context)
        {
        }
    }
}
