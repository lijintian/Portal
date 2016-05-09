using EasyDDD.Infrastructure.Data.MongoDB;
using EasyDDD.Core.Repository;


using Portal.Domain.Aggregates.MenuAgg;
using Portal.Domain.Repositories;

namespace Portal.Infrastructure.Data.MongoDB.Repository
{
    public class MenuRepository : MongoDBRepository<Menu>, IMenuRepository
    {
        public MenuRepository(IRepositoryContext context)
            : base(context)
        {
        }
    }
}
