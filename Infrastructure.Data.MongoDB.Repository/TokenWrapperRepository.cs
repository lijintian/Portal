using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using Portal.Domain.Aggregates.TokenWrapperAgg;
using Portal.Domain.Repositories;
using EasyDDD.Infrastructure.Data.MongoDB;
using EasyDDD.Core.Repository;

namespace Portal.Infrastructure.Data.MongoDB.Repository
{
    /// <summary>
    /// 表示Token访问仓储实现
    /// </summary>
    public class TokenWrapperRepository : MongoDBRepository<TokenWrapper>, ITokenWrapperRepository
    {
        public TokenWrapperRepository(IRepositoryContext context)
            : base(context)
        {
        }
    }
}
