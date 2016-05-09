using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using Portal.Domain.Aggregates.SynchronizationAgg;
using Portal.Domain.Repositories;
using EasyDDD.Infrastructure.Data.MongoDB;
using EasyDDD.Core.Repository;

namespace Portal.Infrastructure.Data.MongoDB.Repository
{
    /// <summary>
    /// 表示同步数据仓储实现
    /// </summary>
    public class SynchronizationInfoRepository : MongoDBRepository<SynchronizationInfo>, ISynchronizationInfoRepository
    {
        public SynchronizationInfoRepository(IRepositoryContext context)
            : base(context)
        {

        }
    }
}
