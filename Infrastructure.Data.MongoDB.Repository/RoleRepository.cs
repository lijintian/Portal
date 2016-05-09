


using EasyDDD.Core.Repository;
using EasyDDD.Infrastructure.Data.MongoDB;
using Portal.Domain.Aggregates.RoleAgg;
using Portal.Domain.Repositories;

namespace Portal.Infrastructure.Data.MongoDB.Repository
{
    public class RoleRepository : MongoDBRepository<Role>, IRoleRepository
    {
        public RoleRepository(IRepositoryContext context)
            : base(context)
        {
        }
    }
}
