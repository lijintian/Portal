using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Domain.Aggregates.TokenWrapperAgg.Strategies
{
    /// <summary>
    /// 刷新Token生成策略实现
    /// </summary>
    class RefreshTokenValueGenerateStrategy : IRefreshTokenValueGenerateStrategy
    {
        public string Generate()
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(Guid.NewGuid().ToString()));
        }
    }
}
