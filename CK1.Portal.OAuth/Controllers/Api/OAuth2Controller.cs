using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Http.ModelBinding;
using System.Web.ModelBinding;
using Portal.Applications.Services;
using Portal.Dto;
using Portal.Dto.Response;
using Portal.Infrastructure.Exceptions;
using Newtonsoft.Json;

namespace Portal.OAuth.Controllers.Api
{
    /// <summary>
    /// 表示OAuth2对外API
    /// </summary>
    public class OAuth2Controller : ApiController
    {
        private readonly ITokenManagerService _tokenManagerService;

        /// <summary>
        /// 实例化OAuth2控制器
        /// </summary>
        public OAuth2Controller(ITokenManagerService tokenManagerService)
        {
            this._tokenManagerService = tokenManagerService;
        }

        /// <summary>
        /// 使用授权码或刷新Token获取新的访问Token
        /// </summary>

        [Route("oauth2/token")]
        [HttpPost]
        [ResponseType(typeof (GetTokenResponse))]
        public IHttpActionResult Token(string client_id,
            string client_secret, string redirect_uri, string grant_type,
            string code = "", string refresh_token = "")
        {
            try
            {
                var token = this._tokenManagerService.RequstAccessToken(client_id, client_secret, redirect_uri,
                    grant_type, code, refresh_token);
                return this.Ok(new GetTokenResponse()
                {
                    AccessToken = token.AccessToken,
                    CustomerId = token.CustomerIdentity,
                    AccessTokenExpiresIn = (long)(token.AccessTokenExpiredTime - token.CreatedOn).TotalMinutes,
                    RefreshTokenExpiresIn = (long)(token.RefreshTokenExpiredTime - token.CreatedOn).TotalMinutes,
                    RefreshToken = token.RefreshToken
                });
            }
            catch (PortalException ex)
            {
                var getTokenError = new GetTokenErrorResponse()
                {
                    Code = ex.ErrorCode,
                    Msg = ex.Message,
                    Request = "POST " + this.Request.RequestUri.PathAndQuery
                };
                return this.BadRequest(getTokenError.ToJson());
            }
        }

        /// <summary>
        /// 表示验证Token Api
        /// </summary>
        [Route("oauth2/tokenauth")]
        [HttpPost]
        [ResponseType(typeof(TokenValidateResult))]
        public IHttpActionResult Authentication(string access_token, string api_code)
        {
            try
            {
                return this.Ok(this._tokenManagerService.ValidateToken(access_token, api_code));
            }
            catch (PortalException ex)
            {
                return this.BadRequest(string.Format("{{ code:{0}, message:{1}, request:{2} }}", ex.ErrorCode, ex.Message, this.Request.RequestUri));
            }
        }
    }
}
