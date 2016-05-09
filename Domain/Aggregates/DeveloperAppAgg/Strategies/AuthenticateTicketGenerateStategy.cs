using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Domain.Aggregates.DeveloperAppAgg.Strategies
{
    /// <summary>
    /// 开发者应用验证信息默认生成
    /// </summary>
    public class AuthenticateTicketGenerateStategy : IAuthenticateTicketGenerateStategy
    {
        public string GenerateClientId()
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(Guid.NewGuid().ToString()));
        }

        public string GenreateSecret()
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(Guid.NewGuid().ToString() + "-" + Guid.NewGuid().ToString()));
        }
    }
}
