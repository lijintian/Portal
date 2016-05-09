using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Portal.Domain.Aggregates.ApiUserAgg;
using Portal.Domain.Aggregates.SynchronizationAgg;
using EasyDDD.Core.Repository;

namespace Portal.Domain.Repositories
{
    /// <summary>
    /// 表示同步数据仓储接口
    /// </summary>
    public interface ISynchronizationInfoRepository : IRepository<SynchronizationInfo>
    {
    }
}
