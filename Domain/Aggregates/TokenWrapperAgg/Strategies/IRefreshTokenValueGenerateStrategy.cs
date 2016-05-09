using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Domain.Aggregates.TokenWrapperAgg.Strategies
{
    /// <summary>
    /// 表示刷新Token产生策略
    /// </summary>
    public interface IRefreshTokenValueGenerateStrategy
    {
        string Generate();
    }
}
