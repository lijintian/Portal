using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Portal.Domain.Aggregates.DeveloperAppAgg;
using Portal.Domain.Aggregates.PermissionAgg;
using Portal.Domain.Aggregates.TokenWrapperAgg;
using Portal.Domain.Aggregates.UserAgg;
using Portal.Domain.Model;
using Portal.Domain.Repositories;
using Portal.Domain.Specification.DeveloperApp;
using Portal.Domain.Specification.Permission;
using Portal.Domain.Specification.TokenWrapper;
using EasyDDD.Core.Repository;

namespace Portal.Domain.Services.Impl
{
    /// <summary>
    /// 表示内部用户Token产生服务实现
    /// </summary>
    public class InteralApiUserTokenGenerateService : IInteralApiUserTokenGenerateService
    {
        private readonly IDeveloperAppRepository _developerAppRepository;
        private readonly IPermissionRepository _permissionRepository;
        private readonly ITokenWrapperRepository _tokenWrapperRepository;
        private readonly IRepositoryContext _dbContext;

        public InteralApiUserTokenGenerateService(IDeveloperAppRepository developerAppRepository,
            ITokenWrapperRepository tokenWrapperRepository,
            IPermissionRepository permissionRepository,
            IRepositoryContext context
            )
        {
            this._developerAppRepository = developerAppRepository;
            this._tokenWrapperRepository = tokenWrapperRepository;
            this._permissionRepository = permissionRepository;
            this._dbContext = context;
        }

        public void Generate(User user)
        {
            var app = this._developerAppRepository.Get(new DeveloperAppUserIdSpecification(user.LoginName));

            if (app == null)
            {
                if (user.Permissions.Any())
                {
                    app = this.CreateNewApp(user);
                    this.CreateNewToken(app);
                }
            }
            else
            {
                this.UpdateAppPermission(app, user);
                var token = this._tokenWrapperRepository.Get(new TokenClientIdSpecification(app.ClientId));
                if (token == null)
                {
                    if (user.Permissions.Any())
                    {
                        this.CreateNewToken(app);
                    }
                }
                else
                {
                    this.UpdateTokenPermissions(token, app.ApprovedList.ToArray());
                }
            }

            this._dbContext.Commit();
        }

        private DeveloperApp CreateNewApp(User user)
        {
            var app = new DeveloperApp(user.LoginName, user.LoginName + "_InteranlApp", "https://www.internal.com", false);
            var apiPermissions = this._permissionRepository.GetApiPermssinsByCodeList(user.Permissions.ToArray());
            app.AddApprovedPermissions(apiPermissions.Select(item => new ReferenceApiPermssionInfo(item.Code, item.IsOpened, item.IsCustomerGranted)));

            this._developerAppRepository.Add(app);
            return app;
        }

        private void UpdateAppPermission(DeveloperApp app, User user)
        {
            if (user.Permissions.Any())
            {
                var apiPermissions = this._permissionRepository.GetApiPermssinsByCodeList(user.Permissions.ToArray());
                app.AddApprovedPermissions(apiPermissions.Select(item => new ReferenceApiPermssionInfo(item.Code, item.IsOpened, item.IsCustomerGranted)));
            }
            else
            {
                app.ClearApprovedPermissions();
            }
            this._developerAppRepository.Update(app);
        }

        private void CreateNewToken(DeveloperApp app)
        {
            var token = TokenWrapper.New(app, app.ApprovedList.ToArray());
            this._tokenWrapperRepository.Add(token);

        }

        private void UpdateTokenPermissions(TokenWrapper token, ReferenceApiPermssionInfo[] permissions)
        {
            token.ResetPermissions(permissions);
            this._tokenWrapperRepository.Update(token);
        }
    }
}
