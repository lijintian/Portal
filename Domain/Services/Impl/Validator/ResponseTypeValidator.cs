using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Portal.Infrastructure.Exceptions;

namespace Portal.Domain.Services.Impl.Validator
{
    /// <summary>
    /// 表示授权响应类型验证
    /// </summary>
    class ResponseTypeValidator : Validator
    {
        private const string Code = "code";
        private const string Token = "token";
        private readonly string _responseType;
        public ResponseTypeValidator(string responseType)
        {
            this._responseType = responseType;
        }

        public override void Validate()
        {
            if (string.IsNullOrEmpty(_responseType))
            {
                throw new PortalException(ErrorCodes.StringCodes.CustomerAuthorizationResponseTypeMissing, ErrorMessage.CustomerAuthorizationResponseTypeMissing);
            }
            if (!_responseType.Equals(Code, StringComparison.OrdinalIgnoreCase) && !_responseType.Equals(Token, StringComparison.OrdinalIgnoreCase))
            {
                throw new PortalException(ErrorCodes.StringCodes.CustomerAuthorizationUnsupportType, ErrorMessage.CustomerAuthorizationUnsupportType);
            }
        }
    }
}
