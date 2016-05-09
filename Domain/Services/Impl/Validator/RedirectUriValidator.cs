using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Portal.Domain.Repositories;
using Portal.Domain.Specification.DeveloperApp;
using Portal.Infrastructure.Exceptions;

namespace Portal.Domain.Services.Impl.Validator
{
    /// <summary>
    /// 表示重定向地址验证
    /// </summary>
    public class RedirectUriValidator : Validator
    {
        private readonly string _redirectUri;
        private readonly string _registerCallbackUrl;
        public RedirectUriValidator(string redirectUri, string registerCallbackUrl)
        {
            this._redirectUri = redirectUri;
            this._registerCallbackUrl = registerCallbackUrl;
        }
        public override void Validate()
        {
            if (string.IsNullOrEmpty(this._redirectUri))
            {
                throw new PortalException(ErrorCodes.StringCodes.CustomerAuthorizationRedirectUriMissing, ErrorMessage.CustomerAuthorizationRedirectUriMissing);
            }
           
            if (!this.IsCognated(this._redirectUri, this._registerCallbackUrl))
            {
                throw new PortalException(ErrorCodes.StringCodes.CustomerAuthorizationRedirectUrlIsNotCognatedWithRegisterCallbcakUrl,
                    ErrorMessage.CustomerAuthorizationRedirectUrlIsNotCognatedWithRegisterCallbcakUrl);
            }
        }

        private bool IsCognated(string redirectUri, string callbackUrl)
        {
            try
            {
                var redirect = new Uri(redirectUri);
                var callback = new Uri(callbackUrl);

                return redirect.Scheme == callback.Scheme && redirect.Host == callback.Host && redirect.Port == callback.Port;
            }
            catch
            {
                throw new PortalException(ErrorCodes.StringCodes.RequestAccessTokenCallbackInvalidFormat, 
                    ErrorMessage.RequestAccessTokenCallbackInvalidFormat);
            }
        }
    }
}
