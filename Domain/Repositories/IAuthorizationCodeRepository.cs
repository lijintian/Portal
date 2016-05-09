using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Portal.Domain.Aggregates.AuthorizationCodeAgg;
using EasyDDD.Core.Repository;

namespace Portal.Domain.Repositories
{
    /// <summary>
    /// 表示授权码访问仓储实现
    /// </summary>
    public interface IAuthorizationCodeRepository : IRepository<AuthorizationCode>
    {

    }
}
