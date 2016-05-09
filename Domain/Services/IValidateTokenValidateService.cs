using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Domain.Services
{
    /// <summary>
    /// 表示token验证时的验证服务
    /// </summary>
    public interface IValidateTokenValidateService
    {
        void Validate(string token, string apiCode);
    }
}
