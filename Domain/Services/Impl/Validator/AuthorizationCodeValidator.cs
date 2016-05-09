using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Portal.Domain.Repositories;
using Portal.Domain.Specification.AuthorizationCode;
using Portal.Infrastructure.Exceptions;

namespace Portal.Domain.Services.Impl.Validator
{
    /// <summary>
    /// 表示授权码验证
    /// </summary>
    public class AuthorizationCodeValidator : Validator
    {
        private readonly string _code;
        private readonly string _clientId;
        private readonly IAuthorizationCodeRepository _repository;
        public AuthorizationCodeValidator(string code, string clientId, IAuthorizationCodeRepository repository)
        {
            this._code = code;
            this._clientId = clientId;
            this._repository = repository;
        }
        public override void Validate()
        {
            if (string.IsNullOrEmpty(this._code))
            {
                throw new PortalException(ErrorCodes.StringCodes.RequestAccessTokenAuthorizationCodeMissing, ErrorMessage.RequestAccessTokenAuthorizationCodeMissing);
            }

            var authCode = this._repository.Get(new AuthorizationCodeSpecification(this._code));
           
            if (authCode == null)
            {
                throw new PortalException(ErrorCodes.StringCodes.RequestAccessTokenAuthorizationCodeNotExist, ErrorMessage.RequestAccessTokenAuthorizationCodeNotExist);
            }

            if (!authCode.ClientId.Equals(this._clientId, StringComparison.Ordinal))
            {
                throw new PortalException(ErrorCodes.StringCodes.RequestAccessTokenAuthorizationCodeAppNotMatch, ErrorMessage.RequestAccessTokenAuthorizationCodeAppNotMatch);
            }
            
            if (authCode.IsInValid())
            {
                throw new PortalException(ErrorCodes.StringCodes.RequestAccessTokenAuthorizationCodeError, ErrorMessage.RequestAccessTokenAuthorizationCodeError);
            }
        }
    }
}
