using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using Portal.Domain.Aggregates.AuthorizationCodeAgg;
using Portal.Domain.Repositories;
using EasyDDD.Infrastructure.Data.MongoDB;
using EasyDDD.Core.Repository;

namespace Portal.Infrastructure.Data.MongoDB.Repository
{
    /// <summary>
    /// 表示授权码仓储实现
    /// </summary>
    public class AuthorizationCodeRepository : MongoDBRepository<AuthorizationCode>, IAuthorizationCodeRepository
    {
        public AuthorizationCodeRepository(IRepositoryContext context)
            : base(context)
        {

        }
    }
}
