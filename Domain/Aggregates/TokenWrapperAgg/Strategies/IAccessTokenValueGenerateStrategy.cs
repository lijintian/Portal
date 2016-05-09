using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Domain.Aggregates.TokenWrapperAgg.Strategies
{
    /// <summary>
    /// 表示Access Token值生成策略
    /// </summary>
    public interface IAccessTokenValueGenerateStrategy
    {
        string Generate();
    }
}
