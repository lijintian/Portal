using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Portal.Domain.Model;

namespace Portal.Domain.Services
{
    /// <summary>
    /// 表示请求访问Token验证服务
    /// </summary>
    public interface IRequstAccessTokenValidateService
    {
        RequstAccessTokenValidateResult Validate(string clientId, string clientSecret, string redirectUrl, string grantType, string code, string refreshToken);
    }
}
