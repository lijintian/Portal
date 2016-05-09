using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Portal.Domain.Aggregates.DeveloperAppAgg;
using EasyDDD.Core.Repository;

namespace Portal.Domain.Repositories
{
    /// <summary>
    /// 表示开发者应用访问仓储
    /// </summary>
    public interface IDeveloperAppRepository : IRepository<DeveloperApp>
    {

    }
}
