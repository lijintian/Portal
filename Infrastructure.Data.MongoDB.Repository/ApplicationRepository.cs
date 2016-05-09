


using EasyDDD.Core.Repository;
using EasyDDD.Infrastructure.Data.MongoDB;
using Portal.Domain.Aggregates.ApplictionAgg;
using Portal.Domain.Repositories;

namespace Portal.Infrastructure.Data.MongoDB.Repository
{
    public class ApplicationRepository : MongoDBRepository<Application>, IApplicationRepository
    {
        public ApplicationRepository(IRepositoryContext context)
            : base(context)
        {
        }

    }
}
