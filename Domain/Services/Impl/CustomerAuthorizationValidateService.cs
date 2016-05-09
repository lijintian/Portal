using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Portal.Domain.Aggregates.PermissionAgg;
using Portal.Domain.Model;
using Portal.Domain.Repositories;
using Portal.Domain.Services.Impl.Validator;
using Portal.Domain.Specification.ApiPermissionGroup;
using Portal.Domain.Specification.DeveloperApp;
using Portal.Domain.Specification.Permission;
using Portal.Infrastructure.Exceptions;

namespace Portal.Domain.Services.Impl
{
    /// <summary>
    /// 授权验证服务实现
    /// </summary>
    public class CustomerAuthorizationValidateService : ICustomerAuthorizationValidateService
    {
        private const string Code = "code";

        private readonly IDeveloperAppRepository _developerAppRepository;
        private readonly IPermissionRepository _permissionRepository;
        private readonly IUserRepository _userRepository;
        private readonly IApiPermissionGroupRepository _groupRepository;
        public CustomerAuthorizationValidateService(IDeveloperAppRepository developerAppRepository, 
            IPermissionRepository permissionRepository, 
            IApiPermissionGroupRepository groupRepository,
            IUserRepository userRepository)
        {
            this._developerAppRepository = developerAppRepository;
            this._permissionRepository = permissionRepository;
            this._groupRepository = groupRepository;
            this._userRepository = userRepository;
        }

        public CustomerAuthValidateResult Validate(string responseType, string redirectUri, string clientId, string scope)
        {
            new ResponseTypeValidator(responseType).Validate();
            new ClientIdValidator(clientId, this._developerAppRepository, this._userRepository).Validate();

            var app = this._developerAppRepository.Get(new DeveloperAppClientIdSpecification(clientId));
            new RedirectUriValidator(redirectUri, app.CallbackUrl).Validate();
            new ScopeValidator(scope, app.ApprovedGroupList.ToArray()).Validate();

            var groupCodes = scope.Split(',');
            var groups = this._groupRepository.GetList(new ApiPermissionGroupCodeListSpecification(groupCodes));
            var apiCodes = new List<string>();
            Array.ForEach(groups.Select(item => item.Permissions).ToArray(), item => apiCodes.AddRange(item));
            var permissions = this._permissionRepository.GetList(new PermissionCodeListSpecification(apiCodes.ToArray()));
            var apiPermissions = Array.ConvertAll(permissions.ToArray(), item => (ApiPermission)item);

            return new CustomerAuthValidateResult()
            {
                App = app,
                RefApiPermssionInfos = apiPermissions.Select(item => new ReferenceApiPermssionInfo(item.Code, item.IsOpened, item.IsCustomerGranted)).ToArray(),
                ResponseType = responseType.Equals(Code, StringComparison.OrdinalIgnoreCase) ? ResponseType.Code : ResponseType.Token
            };
        }

      
    }
}
