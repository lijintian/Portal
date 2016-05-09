using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Portal.Dto;

namespace Portal.Applications.Services
{
    /// <summary>
    /// 表示Token管理服务
    /// </summary>
    public interface ITokenManagerService
    {
        /// <summary>
        /// 设置Token启用
        /// </summary>
        /// <param name="id"></param>
        void SetEnable(string id, SysLoggerDto logger);

        /// <summary>
        /// 设置Token禁用
        /// </summary>
        /// <param name="id"></param>
        void SetDisabled(string id, SysLoggerDto logger);

        /// <summary>
        /// 表示分页查询Token列表
        /// </summary>
        FindTokensResponse FindTokens(FindTokensRequest request);

        /// <summary>
        /// 表示直接跳过授权码直接生成访问Token
        /// </summary>
        TokenWrapperDto DirectGetAccessToken(string clientId, string customerId, string redirectUrl, string scope);

        /// <summary>
        /// 请求访问Token, 通过授权码或刷新Token方式 
        /// </summary>
        /// <param name="clientId">开发者应用Id</param>
        /// <param name="clientSecret">开发者应用密码</param>
        /// <param name="redirectUrl">重定向地址</param>
        /// <param name="grantType">授权类型</param>
        /// <param name="code">权限码</param>
        /// <param name="refreshToken">刷新Token</param>
        /// <param name="customerId">客户标识</param>
        /// <returns>新的Token包装信息</returns>
        TokenWrapperDto RequstAccessToken(string clientId, string clientSecret, string redirectUrl, string grantType, string code = "", string refreshToken = "");

        /// <summary>
        /// 验证Access Token
        /// </summary>
        /// <param name="accessToken">访问Token</param>
        /// <param name="apiPermissionCode">权限码</param>
        /// <param name="customerId">访问客户的标识</param>
        /// <returns></returns>
        TokenValidateResult ValidateToken(string accessToken, string apiPermissionCode);
    }
}
