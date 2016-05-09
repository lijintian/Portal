using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Portal.Infrastructure.Exceptions;

namespace Portal.Domain.Services.Impl.Validator
{
    /// <summary>
    /// 授权类型验证
    /// </summary>
    class GrantTypeValidator : Validator
    {
        public static readonly string AuthorizationCode = "authorization_code";
        public static readonly string RefreshToken = "refresh_token";
        private readonly string _type;
        public GrantTypeValidator(string type)
        {
            this._type = type;
        }
        public override void Validate()
        {
            if (string.IsNullOrEmpty(this._type))
            {
                throw new PortalException(ErrorCodes.StringCodes.RequestAccessTokenGrantTypeMissing, ErrorMessage.RequestAccessTokenGrantTypeMissing);
            }
            if (!this._type.Equals(AuthorizationCode, StringComparison.OrdinalIgnoreCase) &&
                !this._type.Equals(RefreshToken, StringComparison.OrdinalIgnoreCase))
            {
                throw new PortalException(ErrorCodes.StringCodes.RequestAccessTokenGrantTypeError, ErrorMessage.RequestAccessTokenGrantTypeError);
            }

        }
    }
}
