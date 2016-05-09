using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Portal.Domain.Repositories;
using Portal.Domain.Specification.TokenWrapper;
using Portal.Infrastructure.Exceptions;

namespace Portal.Domain.Services.Impl.Validator
{
    /// <summary>
    /// 表示Refresh Token验证
    /// </summary>
    class RefreshTokenValidator : Validator
    {
        private readonly string _refreshToken;
        private readonly string _clientId;
        private readonly ITokenWrapperRepository _repository;
        public RefreshTokenValidator(string refreshToken, string clientId, ITokenWrapperRepository repository)
        {
            this._refreshToken = refreshToken;
            this._clientId = clientId;
            this._repository = repository;
        }

        public override void Validate()
        {
            if (string.IsNullOrEmpty(this._refreshToken))
            {
                throw new PortalException(ErrorCodes.StringCodes.RequestAccessTokenAuthorizationRefreshTokenMissing, 
                    ErrorMessage.RequestAccessTokenAuthorizationRefreshTokenMissing);
            }

            var token = this._repository.Get(new RefreshTokenSpecification(this._refreshToken));
            if (token == null)
            {
                throw new PortalException(ErrorCodes.StringCodes.RequestAccessTokenAuthorizationRefreshTokenNotExist,
                    ErrorMessage.RequestAccessTokenAuthorizationRefreshTokenNotExist);
            }
            if (!token.ClientId.Equals(this._clientId, StringComparison.Ordinal))
            {
                throw new PortalException(ErrorCodes.StringCodes.RequestAccessTokenAuthorizationRefreshTokenNotMatch,
                    ErrorMessage.RequestAccessTokenAuthorizationRefreshTokenNotMatch);
            }
            if (!token.IsAllowUseRefreshToken())
            {
                throw new PortalException(ErrorCodes.StringCodes.RequestAccessTokenAuthorizationRefreshTokenError,
                    ErrorMessage.RequestAccessTokenAuthorizationRefreshTokenError);
            }
        }
    }
}
