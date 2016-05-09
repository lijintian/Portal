using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI.WebControls;
using Portal.Dto;
using Portal.Dto.Response;
using Portal.SDK.Common;
using RestSharp;

namespace Portal.SDK.ServiceClient
{
    /// <summary>
    /// 表示Oauth请求客户端
    /// </summary>
    public static class OauthServiceClient
    {
        /// <summary>
        /// 获取Access Oauth token
        /// </summary>
        public static GetTokenResponse GetAccessToken(string clientId,
            string clientSecret, string redirectUri, string grantType,
            string code, string refreshToken)
        {
            var request = HttpClientFactory.CreateRequest(
                   string.Format("oauth2/token?client_id={0}&client_secret={1}&redirect_uri={2}&grant_type={3}&code={4}&refresh_token={5}",
                   clientId, clientSecret, redirectUri, grantType, code, refreshToken),
                   Method.POST);
           
            var client = HttpClientFactory.Create(Constants.PortalOpenApiOauth2BaseAddressKey);
            var response = client.Execute<GetTokenResponse>(request);

            var statsCode = (int)response.StatusCode;
            if (200 <= statsCode && statsCode < 300)
            {
                return response.Data;
            }
            else
            {
                throw new HttpException((int)response.StatusCode, response.Content);
            }
        }

        public static TokenValidateResult ValidateToken(string token, string apiCode)
        {
            var request = HttpClientFactory.CreateRequest(
                  string.Format("oauth2/tokenauth?access_token={0}&api_code={1}", token, apiCode),
                  Method.POST);

            var client = HttpClientFactory.Create(Constants.PortalOpenApiOauth2BaseAddressKey);
            var response = client.Execute<TokenValidateResult>(request);
            var statsCode = (int)response.StatusCode;
            if (200 <= statsCode && statsCode < 300 )
            {
                return response.Data;
            }
            else
            {
                throw new HttpException((int)response.StatusCode, response.Content);
            }
        }
    }
}
