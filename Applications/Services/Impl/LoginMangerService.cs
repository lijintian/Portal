using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Portal.Domain.Model;
using Portal.Domain.Model.Strategies;
using Portal.Domain.Repositories;
using Portal.Dto;
using Portal.Domain.Services;
using Portal.Infrastructure.Exceptions;
using UserIdentity = Portal.Dto.UserIdentity;
using EasyDDD.Core.Repository;
using EasyDDD.Infrastructure.Crosscutting.Helpers;
using EasyDDD.Infrastructure.Crosscutting;

namespace Portal.Applications.Services.Impl
{
    /// <summary>
    /// 登录服务实现
    /// </summary>
    public class LoginMangerService : ServiceBase, ILoginMangerService
    {
        private readonly ILoginService _loginService;
        private readonly IUserManagerService _userManagerService;
        public LoginMangerService(IRepositoryContext context, ILoginService loginService, IUserManagerService userManagerService)
            : base(context)
        {
            this._loginService = loginService;
            this._userManagerService = userManagerService;
        }

        public UserIdentity Login(LoginInfo loginInfo, SysLoggerDto logger)
        {
            logger.CreatedBy = loginInfo.Identity;
            Check.Argument.IsNotNull(loginInfo, "loginInfo");
            if (string.IsNullOrWhiteSpace(loginInfo.Source))
            {
                throw new PortalValidateException(ErrorCodes.StringCodes.SourceCannotBeNull, ErrorMessage.SourceCannotBeNull);
            }
            var type = loginInfo.IsHashPassword ? LoginType.HashPassword : LoginType.Password;
            var identity = this._loginService.Login(loginInfo.Identity, loginInfo.Password, type);
            LoggerService.Create(SysLoggerType.Login, logger, "登陆Portal", "登陆Portal：名称【{1}】,来源【{2}】", identity.DisplayName, loginInfo.Source);
            return new UserIdentity(identity.LoginName, identity.DisplayName, UserTypeMapper.MapToDtoUserType(identity.UserType), identity.Token);
        }

        public void LoginOut(string token)
        {
            //clear user info 
            if (!string.IsNullOrEmpty(token))
            {
                CacheManager.Remove(Token.GetCacheTokenKey(token));
            }
            else
            {
                throw new PortalValidateException(ErrorCodes.StringCodes.AccessTokenNotExist, ErrorMessage.AccessTokenNotExist);
            }
        }

        public VerifyTokenResult Verify(string tokenValue)
        {
            Check.Argument.IsNotNull(tokenValue, "tokenValue");

            var token = CacheManager.Get<Token>(Token.GetCacheTokenKey(tokenValue));
            if (token == null)
            {
                return new VerifyTokenResult(false, UserIdentity.AnonymousIdentity);
            }
            else
            {
                CacheManager.Set(Token.GetCacheTokenKey(tokenValue), token, Token.ConfigExpired);
                var user = this._userManagerService.GetByIdentity(token.UserIdentity);
                return new VerifyTokenResult(true, new UserIdentity(user.LoginName, user.DisplayName, user.UserType, tokenValue));
            }
        }
    }
}
