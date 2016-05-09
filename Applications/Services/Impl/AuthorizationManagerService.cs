using System;
using System.Collections.Generic;
using System.Linq;
using Portal.Applications.Events;
using Portal.Domain.Repositories;
using Portal.Domain.Specification.ApiPermissionGroup;
using Portal.Domain.Specification.User;
using Portal.Dto;
using UserType = Portal.Domain.Aggregates.UserAgg.UserType;
using EasyDDD.Infrastructure.Crosscutting.Event;
using EasyDDD.Core.Repository;
using EasyDDD.Infrastructure.Crosscutting.Helpers;
using EasyDDD.Infrastructure.Crosscutting.Transaction;

namespace Portal.Applications.Services.Impl
{
    /// <summary>
    /// 表示授权管理服务实现
    /// </summary>
    public class AuthorizationManagerService : ServiceBase, IAuthorizationManagerService
    {
        #region 初始化
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IApiPermissionGroupRepository _apiGroupRepository;
        private readonly IEventBus _eventBus;

        public AuthorizationManagerService(IRepositoryContext context, IUserRepository userRepository, IRoleRepository roleRepository, IApiPermissionGroupRepository apiGroupRepository, IEventBus eventBus)
            : base(context)
        {
            this._userRepository = userRepository;
            this._roleRepository = roleRepository;
            this._apiGroupRepository = apiGroupRepository;
            this._eventBus = eventBus;
        }
        #endregion

        #region 方法
        public void SaveUserToRoles(string userId, IEnumerable<string> roleCodes, SysLoggerDto logger)
        {
            Check.Argument.IsNotNull(userId, "userId");
            Check.Argument.IsNotNull(roleCodes, "roleCodes");

            using (ITransactionCoordinator coordinator = TransactionCoordinatorFactory.Create(Context, _eventBus))
            {
                var user = this._userRepository.GetByKey(userId);
                user.ClearRoles();
                user.AddRoleRange(roleCodes.ToArray());
                user.UpdatedBy = logger.CreatedBy;
                user.UpdatedOn = DateTime.Now;
                this._userRepository.Update(user);
                this._eventBus.Publish(new UserPermissionChangedEvent(userId));
                coordinator.Commit();
            }
            LoggerService.Create(SysLoggerType.Update, logger, "设置用户角色", "保存用户角色列表成功：用户ID【{1}】,角色代码【{2}】", userId, string.Join(",", roleCodes));
        }

        public void SavePermissionsToRole(string roleId, IEnumerable<string> permissionCodes, SysLoggerDto logger)
        {
            Check.Argument.IsNotNull(roleId, "roleId");
            Check.Argument.IsNotNull(permissionCodes, "permissionCodes");

            using (ITransactionCoordinator coordinator = TransactionCoordinatorFactory.Create(Context, _eventBus))
            {
                var role = this._roleRepository.GetByKey(roleId);

                if (permissionCodes.Any())
                {
                    role.SavePermissions(permissionCodes.ToArray());
                }
                else
                {
                    role.ClearPerissions();
                }
                role.UpdatedBy = logger.CreatedBy;
                role.UpdatedOn = DateTime.Now;
                this._roleRepository.Update(role);
                //所有拥这个角色的用户都要触发权限改变事件
                var users = this.GetUsersByRoleCode(role.Code);
                foreach (var user in users)
                {
                    this._eventBus.Publish(new UserPermissionChangedEvent(user));
                }
                coordinator.Commit();
            }
            LoggerService.Create(SysLoggerType.Update, logger, "设置角色权限", "保存角色权限列表成功：角色ID【{1}】,权限码【{2}】", roleId, string.Join(",", permissionCodes));
        }
        /// <summary>
        /// 保存API权限分组的权限列表
        /// </summary>
        public void SavePermissionsToApiGroup(string code, IEnumerable<string> permissionCodes, SysLoggerDto logger)
        {
            Check.Argument.IsNotNull(code, "code");
            Check.Argument.IsNotNull(permissionCodes, "permissionCodes");
            var group = this._apiGroupRepository.Get(new ApiPermissionGroupCodeSpecification(code));
            if (permissionCodes.Any())
            {
                group.SavePermissions(permissionCodes.ToArray());
            }
            else
            {
                group.ClearPerissions();
            }
            group.UpdatedBy = logger.CreatedBy;
            group.UpdatedOn = DateTime.Now;
            this._apiGroupRepository.Update(group);
            Context.Commit();
            LoggerService.Create(SysLoggerType.Update, logger, "设置API权限分组", "保存API权限分组成功：API权限分组编码【{1}】,权限码【{2}】", code, string.Join(",", permissionCodes));
        }

        public void SaveUserOwnedPermissions(string userId, IEnumerable<string> permissionCodes, SysLoggerDto logger)
        {
            Check.Argument.IsNotNull(userId, "userId");
            Check.Argument.IsNotNull(permissionCodes, "permissionCodes");
            using (ITransactionCoordinator coordinator = TransactionCoordinatorFactory.Create(Context, _eventBus))
            {
                var user = this._userRepository.GetByKey(userId);
                user.ClearPermisssions();
                user.AddPermisssionRange(permissionCodes.ToArray());
                user.UpdatedBy = logger.CreatedBy;
                user.UpdatedOn = DateTime.Now;
                this._userRepository.Update(user);
                this._eventBus.Publish(new UserPermissionChangedEvent(userId));
                if (user.UserType == UserType.InternalApi)
                {
                    this._eventBus.Publish(new InternalApiUserPermssionResetEvent(user));
                }
                coordinator.Commit();
            }
            LoggerService.Create(SysLoggerType.Update, logger, "设置用户权限", "保存用户权限列表成功：用户ID【{1}】,权限码【{2}】", userId, string.Join(",", permissionCodes));
        }
        #endregion

        #region 私有方法
        private IEnumerable<string> GetUsersByRoleCode(string code)
        {
            return this._userRepository.GetList(new UserOwnedRoleSpecification(code)).Select(item => item.Id);
        }
        #endregion
    }
}
