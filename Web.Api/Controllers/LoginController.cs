using System.Web.Http;
using System.Web.Http.Description;
using Portal.Dto;
using Portal.Dto.Response;
using Portal.Applications.Services;
using Portal.Infrastructure.Exceptions;
using Portal.Web.Api.Core;
using Portal.Web.Api.Extends;

namespace Portal.Web.Api.Controllers
{
    /// <summary>
    /// 表示登录服务API
    /// </summary>
    public class LoginController : BaseController
    {
        private ILoginMangerService LoginMangerService { get; set; }
        public LoginController(ILoginMangerService service)
        {
            this.LoginMangerService = service;
        }

        #region 旧版API
        /// <summary>
        /// 登录验证
        /// </summary>
        /// <param name="identity">登录名或员工编号</param>
        /// <param name="password">密码</param>
        /// <param name="source">访问来源</param>
        /// <remarks>注意，这是老版本的API，应禁止调用</remarks>
        [Route("api/login/in")]
        [HttpGet]
        [ResponseType(typeof(LoginInResponse))]
        public IHttpActionResult LoginIn(string identity, string password, string source = "NotSet")
        {
            return UserLogin(identity, password, source, false);
        }

        /// <summary>
        /// 用户注销
        /// </summary>
        /// <remarks>注意，这是老版本的API，应禁止调用</remarks>
        [Route("api/login/out/{token}")]
        [HttpGet]
        [ResponseType(typeof(LoginOutResponse))]
        public IHttpActionResult LoginOut(string token)
        {
            return this.UserLogout(token);
        }

        /// <summary>
        /// 验证标识
        /// </summary>
        [Route("api/login/token/{token}")]
        [HttpGet]
        [ResponseType(typeof(VerifyTokenResponse))]
        public IHttpActionResult VerifyToken(string token)
        {
            return this.ValidateTokenImpl(token);
        }

        #endregion

        #region 新版API
        /// <summary>
        /// 登录验证
        /// </summary>
        /// <param name="identity">登录名或员工编号</param>
        /// <param name="password">密码</param>
        /// <param name="source">访问来源</param>
        [Route("api/v1/login")]
        [HttpPost]
        [ResponseType(typeof(LoginInResponse))]
        public IHttpActionResult Login(string identity, string password, string source)
        {
            return UserLogin(identity, password, source, false);
        }

        /// <summary>
        /// 登录验证
        /// </summary>
        /// <param name="identity">登录名或员工编号</param>
        /// <param name="hash">密码</param>
        /// <param name="source">访问来源</param>
        [Route("api/v1/hashlogin")]
        [HttpPost]
        [ResponseType(typeof(LoginInResponse))]
        public IHttpActionResult HashLogin(string identity, string hash, string source)
        {
            return UserLogin(identity, hash, source, true);
        }

        /// <summary>
        /// 登录验证
        /// </summary>
        [Route("api/v1/logout/{token}")]
        [HttpPost]
        [ResponseType(typeof(LoginOutResponse))]
        public IHttpActionResult Logout(string token)
        {
            return this.UserLogout(token);
        }

        /// <summary>
        /// 验证标识
        /// </summary>
        [Route("api/v1/token/{token}")]
        [HttpPost]
        [ResponseType(typeof(VerifyTokenResponse))]
        public IHttpActionResult ValidateToken(string token)
        {
            return this.ValidateTokenImpl(token);
        }

        #endregion

        #region 私有方法

        private IHttpActionResult UserLogout(string token)
        {
            try
            {
                this.LoginMangerService.LoginOut(token);
                return this.Ok(new LoginOutResponse());
            }
            catch (PortalException ex)
            {
                return this.BadRequest(ex.Message);
            }
        }
        private IHttpActionResult UserLogin(string identity, string password, string source, bool isHashPassword)
        {
            try
            {
                var userIdentity = this.LoginMangerService.Login(new LoginInfo()
                {
                    Identity = identity,
                    Password = password,
                    Ip = this.Request.GetClientIpAddress(),
                    Source = source,
                    IsHashPassword = isHashPassword
                }, GetLogger(identity));
                return this.Ok(new LoginInResponse()
                {
                    DisplayName = userIdentity.DisplayName,
                    LoginName = userIdentity.LoginName,
                    IsApi = userIdentity.UserType == UserType.InternalApi,
                    IsCustomer = userIdentity.UserType == UserType.Customer,
                    IsEmployee = userIdentity.UserType == UserType.Employee,
                    Token = userIdentity.Token
                });
            }
            catch (PortalException ex)
            {
                return this.BadRequest(ex.Message);
            }
        }

        private IHttpActionResult ValidateTokenImpl(string token)
        {
            try
            {
                var result = this.LoginMangerService.Verify(token);
                return this.Ok(new VerifyTokenResponse()
                {
                    LoginName = result.Identity.LoginName,
                    DisplayName = result.Identity.DisplayName,
                    IsSuccessed = result.IsPassed
                });
            }
            catch (PortalException ex)
            {
                return this.BadRequest(ex.Message);
            }
        }
        #endregion
    }
}
