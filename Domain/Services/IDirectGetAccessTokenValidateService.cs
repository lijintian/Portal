using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Portal.Domain.Model;

namespace Portal.Domain.Services
{
    /// <summary>
    /// 表示直接获取Token请求验证
    /// </summary>
    public interface IDirectGetAccessTokenValidateService
    {
        DirectGetAccessTokenValidateResult Validate(string clientId, string redirectUrl, string scope);
    }
}
