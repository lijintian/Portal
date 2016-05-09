using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Portal.Infrastructure.Exceptions;

namespace Portal.Domain.Services.Impl.Validator
{
    /// <summary>
    /// 开发者密钥验证
    /// </summary>
    public class ClientSecretValidator : Validator
    {
        private readonly string _secret;
        private readonly string _dbSecret;
        public ClientSecretValidator(string secret, string dbSecret)
        {
            this._secret = secret;
            this._dbSecret = dbSecret;
        }

        public override void Validate()
        {
            if (string.IsNullOrEmpty(this._secret))
            {
                throw new PortalException(ErrorCodes.StringCodes.RequestAccessTokenSecretMissing, ErrorMessage.RequestAccessTokenSecretMissing);
            }
            if (!this._secret.Equals(this._dbSecret, StringComparison.Ordinal))
            {
                throw new PortalException(ErrorCodes.StringCodes.RequestAccessTokenSecretNotMatch, ErrorMessage.RequestAccessTokenSecretNotMatch);
            }
        }
    }
}
