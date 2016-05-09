using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Domain.Aggregates.AuthorizationCodeAgg.Strategies
{
    /// <summary>
    /// 表示授权码产生策略
    /// </summary>
    public interface ICodeGenreateStrategy
    {
        string Generate();
    }
}
