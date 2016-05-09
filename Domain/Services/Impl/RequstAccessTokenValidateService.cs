using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Portal.Domain.Model;
using Portal.Domain.Repositories;
using Portal.Domain.Services.Impl.Validator;
using Portal.Domain.Specification.AuthorizationCode;
using Portal.Domain.Specification.DeveloperApp;
using Portal.Domain.Specification.TokenWrapper;

namespace Portal.Domain.Services.Impl
{
    /// <summary>
    /// 表示请求Access Token验证实现
    /// </summary>
    public class RequstAccessTokenValidateService : IRequstAccessTokenValidateService
    {
        private readonly IDeveloperAppRepository _appRepository;
        private readonly IAuthorizationCodeRepository _authRepository;
        private readonly ITokenWrapperRepository _tokenWrapperRepository;
        private readonly IUserRepository _userRepository;
        public RequstAccessTokenValidateService(IDeveloperAppRepository appRepository, 
            IAuthorizationCodeRepository authRepository, 
            ITokenWrapperRepository tokenWrapperRepository,
            IUserRepository userRepository)
        {
            this._appRepository = appRepository;
            this._authRepository = authRepository;
            this._tokenWrapperRepository = tokenWrapperRepository;
            this._userRepository = userRepository;
        }
        public RequstAccessTokenValidateResult Validate(string clientId, string clientSecret, string redirectUrl, string grantType, string code, string refreshToken)
        {
            new ClientIdValidator(clientId, this._appRepository, this._userRepository).Validate();

            var app = this._appRepository.Get(new DeveloperAppClientIdSpecification(clientId));
            new ClientSecretValidator(clientSecret, app.Secret).Validate();
            new RedirectUriValidator(redirectUrl, app.CallbackUrl).Validate();
            new GrantTypeValidator(grantType).Validate();

            GrantType grant;
            ReferenceApiPermssionInfo[] permissons;
            string customerId;
            if (grantType == GrantTypeValidator.AuthorizationCode)
            {
                grant = GrantType.AuthorizationCode;
                new AuthorizationCodeValidator(code, clientId, this._authRepository).Validate();

                var authCode = this._authRepository.Get(new AuthorizationCodeSpecification(code));
                permissons = authCode.Permssions.ToArray();
                customerId = authCode.CustomerIdentity;
            }
            else
            {
                grant = GrantType.RefreshToken;
                new RefreshTokenValidator(refreshToken, clientId, this._tokenWrapperRepository).Validate();
                var token = this._tokenWrapperRepository.Get(new RefreshTokenSpecification(refreshToken));
                permissons = token.ApiPermissionCodes.ToArray();
                customerId = token.CustomerIdentity;
            }
            
            return new RequstAccessTokenValidateResult() { GrantType = grant, App = app, ApiPermssionInfos = permissons, CustomerId = customerId};

        }
    }
}
