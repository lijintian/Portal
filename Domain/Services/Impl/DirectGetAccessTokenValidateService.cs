using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Portal.Domain.Model;
using Portal.Domain.Repositories;
using Portal.Domain.Services.Impl.Validator;
using Portal.Domain.Specification.ApiPermissionGroup;
using Portal.Domain.Specification.DeveloperApp;
using Portal.Domain.Specification.Permission;

namespace Portal.Domain.Services.Impl
{
    /// <summary>
    /// 表示直接获取Token请求验证实现
    /// </summary>
    public class DirectGetAccessTokenValidateService : IDirectGetAccessTokenValidateService
    {
        private readonly IDeveloperAppRepository _appRepository;
        private readonly IPermissionRepository _permissionRepository;
        private readonly IUserRepository _userRepository;
        private readonly IApiPermissionGroupRepository _groupRepository;
        public DirectGetAccessTokenValidateService(IDeveloperAppRepository appRepository, 
            IPermissionRepository permissionRepository,
            IApiPermissionGroupRepository groupRepository,
            IUserRepository userRepository)
        {
            this._appRepository = appRepository;
            this._permissionRepository = permissionRepository;
            this._groupRepository = groupRepository;
            this._userRepository = userRepository;
        }
        public DirectGetAccessTokenValidateResult Validate(string clientId, string redirectUrl, string scope)
        {
            new ClientIdValidator(clientId, this._appRepository, this._userRepository).Validate();

            var app = this._appRepository.Get(new DeveloperAppClientIdSpecification(clientId));
            new RedirectUriValidator(redirectUrl, app.CallbackUrl).Validate();
            new ScopeValidator(scope, app.ApprovedGroupList.ToArray()).Validate();

            var groupCodes = scope.Split(',');
            var groups = this._groupRepository.GetList(new ApiPermissionGroupCodeListSpecification(groupCodes));
            var apiCodes = new List<string>();
            Array.ForEach(groups.Select(item => item.Permissions).ToArray(), item => apiCodes.AddRange(item));
            var result = this._permissionRepository.FindApiPermissionsInPage(1, apiCodes.Count + 1,
                new ApiPermissionCodeListSpecification(apiCodes.ToArray()), null);
            
            return new DirectGetAccessTokenValidateResult()
            {
                App = app,
                ApiPermssionInfos = result.Data.Select(item => new ReferenceApiPermssionInfo(item.Code, item.IsOpened, item.IsCustomerGranted)).ToArray()
            };
        }
    }
}
