using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Domain.Aggregates.AuthorizationCodeAgg.Strategies
{
    /// <summary>
    /// 表示授权码生成策略
    /// </summary>
    class CodeGenreateStrategy : ICodeGenreateStrategy
    {
        public string Generate()
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(Guid.NewGuid().ToString()));
        }
    }
}
