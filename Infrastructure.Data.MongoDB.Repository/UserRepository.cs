


using EasyDDD.Core.Repository;
using EasyDDD.Infrastructure.Data.MongoDB;
using Portal.Domain.Aggregates.UserAgg;
using Portal.Domain.Repositories;

namespace Portal.Infrastructure.Data.MongoDB.Repository
{
    public class UserRepository : MongoDBRepository<User>, IUserRepository
    {
        public UserRepository(IRepositoryContext context)
            : base(context)
        {
        }
    }
}
