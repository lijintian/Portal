using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Portal.Domain.Repositories;
using Portal.Domain.Specification.DeveloperApp;
using Portal.Domain.Specification.User;
using Portal.Infrastructure.Exceptions;

namespace Portal.Domain.Services.Impl.Validator
{
    /// <summary>
    /// 客户Id验证器
    /// </summary>
    public class ClientIdValidator : Validator
    {
        private readonly string _clientId;
        private readonly IDeveloperAppRepository _appRepository;
        private readonly IUserRepository _userRepository;
        public ClientIdValidator(string clientId, IDeveloperAppRepository appRepository, IUserRepository userRepository)
        {
            this._clientId = clientId;
            this._appRepository = appRepository;
            this._userRepository = userRepository;
        }

        public override void Validate()
        {
            if (string.IsNullOrEmpty(this._clientId))
            {
                throw new PortalException(ErrorCodes.StringCodes.CustomerAuthorizationClientIdMissing, ErrorMessage.CustomerAuthorizationClientIdMissing);
            }

            var app = this._appRepository.Get(new DeveloperAppClientIdSpecification(this._clientId));
            if (app == null)
            {
                throw new PortalException(ErrorCodes.StringCodes.CustomerAuthorizationAppNotExist, ErrorMessage.CustomerAuthorizationAppNotExist);
            }
            if (!app.IsEnable())
            {
                throw new PortalException(ErrorCodes.StringCodes.ApplicationStateError, ErrorMessage.ApplicationStateError);
            }

            var user = this._userRepository.Get(new UserLoginNameSpecification(app.UserId));
            if (!user.IsApproved)
            {
                throw new PortalException(ErrorCodes.StringCodes.UserAccountDisable, ErrorMessage.UserAccountDisable);
            }
        }
    }
}
