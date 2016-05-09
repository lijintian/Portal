using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Portal.Domain.Aggregates.UserAgg;
using Portal.Domain.Model;

namespace Portal.Domain.Services
{
    /// <summary>
    /// 表示内部用户Token产生服务
    /// </summary>
    public interface IInteralApiUserTokenGenerateService
    {
        void Generate(User user);
    }
}
