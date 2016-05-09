using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Portal.Domain.Repositories;
using Portal.Domain.Services.Impl.Validator;
using Portal.Domain.Specification.TokenWrapper;
using Portal.Domain.Specification.User;
using Portal.Infrastructure.Exceptions;

namespace Portal.Domain.Services.Impl
{
    /// <summary>
    /// 表示token验证内部验证服务实现
    /// </summary>
    public class ValidateTokenValidateService : IValidateTokenValidateService
    {
        private readonly ITokenWrapperRepository _tokenRepository;
        private readonly IDeveloperAppRepository _appRepository;
        private readonly IUserRepository _userRepository;
        public ValidateTokenValidateService(ITokenWrapperRepository tokenRepository,
            IDeveloperAppRepository appRepository,
            IUserRepository userRepository)
        {
            this._tokenRepository = tokenRepository;
            this._appRepository = appRepository;
            this._userRepository = userRepository;
        }

        public void Validate(string token, string apiCode)
        {
            if (string.IsNullOrEmpty(token))
            {
                throw new PortalException(ErrorCodes.StringCodes.TokenCannotBeNull, 
                    ErrorMessage.TokenCannotBeNull);
            }
            if (string.IsNullOrEmpty(apiCode))
            {
                throw new PortalException(ErrorCodes.StringCodes.PermissionCodeCannotNull,
                    ErrorMessage.PermissionCodeCannotNull);
            }

            var tokenWarpper = this._tokenRepository.Get(new AccessTokenSpecification(token));
            if (tokenWarpper == null)
            {
                throw new PortalException(ErrorCodes.StringCodes.AccessTokenNotExist,
                   ErrorMessage.AccessTokenNotExist);
            }

            if (!tokenWarpper.IsAllowedAccess(apiCode))
            {
                throw new PortalException(ErrorCodes.StringCodes.AccessTokenValidateFail,
                   ErrorMessage.AccessTokenValidateFail);
            }

            var clientId = tokenWarpper.ClientId;
            new ClientIdValidator(clientId, this._appRepository, this._userRepository).Validate();

            if (!string.IsNullOrWhiteSpace(tokenWarpper.CustomerIdentity))
            {
                var customer = this._userRepository.Get(new UserLoginNameSpecification(tokenWarpper.CustomerIdentity));
                if (!customer.IsApproved)
                {
                    throw new PortalException(ErrorCodes.StringCodes.CustomerIsForbiddenAccessTokenFail, ErrorMessage.CustomerIsForbiddenAccessTokenFail);
                }
            }
        }
    }
}
