using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using Portal.Domain.Aggregates.DeveloperAppAgg;
using Portal.Domain.Repositories;
using EasyDDD.Infrastructure.Data.MongoDB;
using EasyDDD.Core.Repository;

namespace Portal.Infrastructure.Data.MongoDB.Repository
{
    /// <summary>
    /// 表示开发者应用数据访问仓储实现
    /// </summary>
    public class DeveloperAppRepository : MongoDBRepository<DeveloperApp>, IDeveloperAppRepository
    {
        public DeveloperAppRepository(IRepositoryContext context)
            : base(context)
        {

        }
    }
}
