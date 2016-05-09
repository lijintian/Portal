using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Portal.Dto;
using Portal.Dto.Request;
using Portal.Dto.Response;

namespace Portal.Applications.Services
{
    /// <summary>
    /// 表示客户授权管理服务，支持OAuth的授权码与隐式授权方式
    /// </summary>
    public interface ICustomerAuthorizationManagerService
    {
        /// <summary>
        /// 验证授权请求
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        AuthorizationRequestValidateResult RequestValidate(AuthorizationRequest request);

        /// <summary>
        /// 请求授权
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        AuthorizationResponse RequestAuthorization(AuthorizationRequest request);
    }
}
