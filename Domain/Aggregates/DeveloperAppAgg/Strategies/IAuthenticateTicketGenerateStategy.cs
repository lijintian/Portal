using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Domain.Aggregates.DeveloperAppAgg.Strategies
{
    /// <summary>
    /// 表示应用验证生成信息
    /// </summary>
    public interface IAuthenticateTicketGenerateStategy
    {
        /// <summary>
        /// 生成应用客户Id
        /// </summary>
        string GenerateClientId();

        /// <summary>
        /// 生成应用访问私密
        /// </summary>
        string GenreateSecret();
    }
}
