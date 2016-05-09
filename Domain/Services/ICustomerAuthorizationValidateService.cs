using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Portal.Domain.Model;

namespace Portal.Domain.Services
{
    /// <summary>
    /// Oauth授权验证服务
    /// </summary>
    public interface ICustomerAuthorizationValidateService
    {
        CustomerAuthValidateResult Validate(string responseType, string redirectUri, string clientId, string scope);
    }
}
