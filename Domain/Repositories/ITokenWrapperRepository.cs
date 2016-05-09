using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Portal.Domain.Aggregates.TokenWrapperAgg;
using EasyDDD.Core.Repository;

namespace Portal.Domain.Repositories
{
    /// <summary>
    /// 表示Token包装访问仓储
    /// </summary>
    public interface ITokenWrapperRepository : IRepository<TokenWrapper>
    {

    }
}
