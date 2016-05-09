using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Caching;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Web.Http;
using System.Web.Http.Filters;
using System.Web.Http.Results;
using Portal.Dto;
using Portal.SDK.ServiceClient;

namespace Portal.WebApi
{
    /// <summary>
    /// 表示Portal访问Token验证
    /// </summary>
    public class PortalAccessTokenAuthenticationFilter : FilterAttribute, IAuthenticationFilter
    {
        private readonly string _apiPermissionCode;
        private static readonly int cacheSpan = GetAccessTokenCacheTime();
        private const string Bearer = "Bearer";

        /// <summary>
        /// 表示验证错误
        /// </summary>
        protected string ValidatedError
        {
            get;
            private set;
        }

        /// <summary>
        /// 实例化新的filter
        /// </summary>
        /// <param name="code"></param>
        public PortalAccessTokenAuthenticationFilter(string code)
        {
            this._apiPermissionCode = code;
            this.ValidatedError = string.Empty;
        }

        public Task AuthenticateAsync(HttpAuthenticationContext context, System.Threading.CancellationToken cancellationToken)
        {
            string token;
            //add cache
            if (this.TryGetAccessToken(context.Request.Headers, out token))
            {
                try
                {
                    var key = GetKey(token, this._apiPermissionCode);
                    var result = MemoryCache.Default.Get(key) as TokenValidateResult;
                    if (result == null)
                    {
                        result = OauthServiceClient.ValidateToken(token, this._apiPermissionCode);

                        var policy = new CacheItemPolicy() { SlidingExpiration = new TimeSpan(0, 0, cacheSpan, 0) };
                        MemoryCache.Default.Add(key, result, policy);
                    }
                    context.Principal =
                        new GenericPrincipal(new AccessTokenIdentity(result.CustomerNo, result.CustomerName, result.AppClientId),
                            new string[0]);
                }
                catch (HttpException ex)
                {
                    this.ValidatedError = ex.Message;
                    ErrorNotify.Notify(ex);
                }
                catch (Exception ex)
                {
                    this.ValidatedError = "sorry validate token fail, please try again later.";
                    ErrorNotify.Notify(ex);
                }

            }
            else
            {
                this.ValidatedError = "please set authenticate http head. such as Authorization: Bearer AccessToken";
            }

            return Task.FromResult<object>(null);
        }

        public Task ChallengeAsync(HttpAuthenticationChallengeContext context, System.Threading.CancellationToken cancellationToken)
        {
            var user = context.ActionContext.ControllerContext.RequestContext.Principal;
            if (user == null || !user.Identity.IsAuthenticated || !(user.Identity is AccessTokenIdentity))
            {
                context.Result = GetChallenge(context, this.ValidatedError);
            }
            
            return Task.FromResult<object>(null);
        }

        /// <summary>
        /// 获取质询
        /// </summary>
        /// <param name="context">验证上下文</param>
        /// <param name="error">错误信息</param>
        /// <returns>质询结算</returns>
        protected virtual IHttpActionResult GetChallenge(HttpAuthenticationChallengeContext context, string error)
        {
            var response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
            response.Headers.Add(Bearer, Convert.ToBase64String(Encoding.UTF8.GetBytes(error)));
            response.RequestMessage = context.Request;
            response.Content = new StringContent(error);
           
            return new ResponseMessageResult(response); 
        }

        private bool TryGetAccessToken(HttpRequestHeaders headers, out string token)
        {
            token = string.Empty;
            var authHeader = headers.Authorization;
            if (authHeader != null && Bearer.Equals(authHeader.Scheme, StringComparison.OrdinalIgnoreCase))
            {
                token = authHeader.Parameter;
                return true;
            }
            return false;
        }

        private static string GetKey(string token, string code)
        {
            return string.Format("AccessTokenAuthentication{0}{1}", token, code);
        }

        private static int GetAccessTokenCacheTime()
        {
            var setting = ConfigurationManager.AppSettings["AccessTokenCacheTime"];
            return string.IsNullOrEmpty(setting) ? 5 : int.Parse(setting);
        }
    }
}
