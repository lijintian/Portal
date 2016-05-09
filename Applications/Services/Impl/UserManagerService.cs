using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Portal.Applications.Events;
using Portal.Domain.Repositories;
using Portal.Domain.Specification.Menu;
using Portal.Domain.Specification.Permission;
using Portal.Domain.Specification.Role;
using Portal.Domain.Specification.User;
using Portal.Dto;
using Portal.Dto.Request;
using Portal.Dto.Response;
using Portal.Infrastructure.Exceptions;
using Portal.Web.Core;
using DomainUser = Portal.Domain.Aggregates.UserAgg.User;
using DomainMenu = Portal.Domain.Aggregates.MenuAgg.Menu;
using EasyDDD.Infrastructure.Crosscutting.Paged;
using EasyDDD.Infrastructure.Crosscutting.Transaction;
using EasyDDD.Core.Specification;
using EasyDDD.Infrastructure.Crosscutting.Event;
using EasyDDD.Core.Repository;
using EasyDDD.Infrastructure.Crosscutting.InversionOfControl;
using EasyDDD.Infrastructure.Crosscutting.Helpers;

namespace Portal.Applications.Services.Impl
{
    /// <summary>
    /// 表示用户管理服务实现
    /// </summary>
    public class UserManagerService : PortalService<User, FindUserRequest, DomainUser>, IUserManagerService
    {
        #region 初始化
        private readonly IEventBus _bus;
        public UserManagerService(
            IRepositoryContext context,
            IEventBus bus,
            IUserRepository userRepository)
            : base(context, userRepository)
        {
            this._bus = bus;
        }
        #endregion

        #region 属性
        private IRoleRepository _roleRepository
        {
            get { return IoC.Resolve<IRoleRepository>(); }
        }
        private IPermissionRepository _permissionRepository
        {
            get { return IoC.Resolve<IPermissionRepository>(); }
        }
        private IMenuRepository _menuRepository
        {
            get { return IoC.Resolve<IMenuRepository>(); }
        }
        private IMenuManagerService _menuService { get { return IoC.Resolve<IMenuManagerService>(); } }
        private IApplicationManagerService _appManagerService { get { return IoC.Resolve<IApplicationManagerService>(); } }
        #endregion

        #region 获取
        public FindUserResponse FindUsers(FindUserRequest request)
        {
            Check.Argument.IsNotNull(request, "request");
            var order = new Dictionary<Expression<Func<DomainUser, object>>, SortOrder>();
            order.Add(item => item.CreatedOn, SortOrder.Descending);
            return Search<FindUserResponse>(request, order);
        }
        /// <summary>
        /// 获取内部和外部Api账号
        /// </summary>
        /// <returns></returns>
        public IEnumerable<User> GetApiUserList()
        {
            var result = this._repository.GetList(new UserTypeListSpecification(true));
            return Array.ConvertAll(result.ToArray(), item => DtoDomainMapper.ConvertToDto(item));
        }

        public User GetById(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return null;
            }
            return DtoDomainMapper.ConvertToDto(this._repository.GetByKey(id));
        }

        public bool IsUniqueLoginName(string loginName)
        {
            CheckArgument.IsNotNullOrEmpty(loginName, "登录名称");
            return !this._repository.Exists(new UserLoginNameSpecification(loginName).Or(new EmployeeNoSpecification(loginName)));
        }

        public User GetByIdentity(string identity)
        {
            if (string.IsNullOrEmpty(identity))
            {
                return null;
            }
            var domainUser = this.GetDomainUserByIdentity(identity);
            if (domainUser == null)
            {
                return null;
            }

            return DtoDomainMapper.ConvertToDto(domainUser);
        }

        public GetUserResponse GetPackageUserInfo(string identity)
        {
            if (string.IsNullOrEmpty(identity))
            {
                return null;
            }

            var user = this.GetDomainUserByIdentity(identity);
            if (user == null)
            {
                return null;
            }

            return new GetUserResponse()
            {
                LoginName = user.LoginName,
                Desc = user.Desc,
                DisplayName = user.DisplayName,
                EmployeeNo = user.EmployeeNo,
                ClientNo = user.ClientNo,
                Email = user.Email,
                IsApproved = user.IsApproved,
                UserType = user.UserType.ToString(),
                RoleCodes = user.Roles,
                PermissionCodes = OwnedPermissionCodes(user)
            };
        }
        public IEnumerable<Role> GetRoles(string identity)
        {
            if (string.IsNullOrEmpty(identity))
            {
                return Enumerable.Empty<Role>();
            }

            var user = this.GetDomainUserByIdentity(identity);
            if (user == null)
            {
                return Enumerable.Empty<Role>();
            }

            var roles = _roleRepository.GetList(new RoleCodeListSpecification(user.Roles.ToArray()));
            return Array.ConvertAll(roles.ToArray(), item => DtoDomainMapper.ConvertToDto(item));
        }

        public List<Menu> GetUserMenus(string appId, string identity)
        {
            List<Menu> result = new List<Menu>();
            var menuList = this.GetUserMenus(true, appId, identity);
            IEnumerable<Application> appList = this._appManagerService.GetAll();
            var parentId = "0";
            var otherParentId = "none999";
            if (menuList == null || !menuList.Any())
            {
                return null;
            }
            if (appList.Any())
            {
                foreach (var item in appList)
                {
                    var tempList = menuList.Where(u => u.ApplicationId == item.Id && u.IsFirstParent());
                    if (tempList.Any())
                    {
                        result.Add(new Menu() { Id = item.Id, Name = item.CnName, ParentId = parentId });
                        foreach (var menuItem in tempList)
                        {
                            menuItem.ParentId = item.Id;
                        }
                    }
                }
            }
            var tempList2 = menuList.Where(u => string.IsNullOrEmpty(u.ApplicationId) && u.IsFirstParent());
            if (tempList2.Any())
            {
                result.Add(new Menu() { Id = otherParentId, Name = "其他", ParentId = parentId });
                foreach (var menuItem in tempList2)
                {
                    menuItem.ParentId = otherParentId;
                }
            }
            result.AddRange(menuList);
            return result;
        }

        public IEnumerable<Menu> GetUserMenus(bool getAll, string appId, string identity)
        {
            if (string.IsNullOrWhiteSpace(identity))
            {
                return Enumerable.Empty<Menu>();
            }
            var ownedPerCodes = this.GetOwnedPermissionCodes(identity);
            var ownedPerCodeList = ownedPerCodes.ToList();
            if (ownedPerCodeList.Any())
            {
                //if (!string.IsNullOrEmpty(appId))
                //{
                //    //有指定应用id, 只返回属于该应用的菜单列表
                //    var appPerms = this._permissionRepository.GetList(new PermissionApplicationIdSpecification(appId).And(new PermissionCodeListSpecification(ownedPerCodeList.ToArray()))).ToList();
                //    ownedPerCodeList = appPerms.Select(item => item.Code).ToList();
                //}
                var menuList = this._menuService.GetList(new FindMenuRequest() { ApplicationId = getAll ? string.Empty : appId, IsShare = true, PermissionCodeList = ownedPerCodeList });
                List<Menu> list = new List<Menu>();
                if (menuList.Any())
                {
                    foreach (var menu in menuList)
                    {
                        if (IsShow(menu, menuList))
                        {
                            list.Add(menu);
                        }
                    }
                }
                if (getAll && list.Any())
                {
                    List<Application> appList = this._appManagerService.GetAll().ToList();
                    if (appList.Any())
                    {
                        foreach (var item in list)
                        {
                            if (item.IsAbsoluteUrl || string.IsNullOrEmpty(item.ApplicationId) || string.IsNullOrEmpty(item.Url) || item.ApplicationId == appId)
                            {
                                continue;
                            }
                            var appInfo = appList.FirstOrDefault(u => u.Id == item.ApplicationId);
                            if (appInfo == null)
                            {
                                continue;
                            }
                            item.SetUrl(appInfo.Url);
                        }
                    }
                }
                return list;
            }
            return Enumerable.Empty<Menu>();
        }

        public bool HasPermission(string identity, string permissionCode)
        {
            CheckArgument.IsNotNullOrEmpty(identity, "登录名称");
            CheckArgument.IsNotNullOrEmpty(permissionCode, "权限码");
            return this.GetOwnedPermissions(identity).FirstOrDefault(item => item.Code == permissionCode) != null;
        }

        public bool HasPermissions(string identity, string[] permissionCodes)
        {
            CheckArgument.IsNotNullOrEmpty(identity, "登录名称");
            CheckArgument.IsNotNullOrEmpty(permissionCodes, "权限码");
            return this.GetOwnedPermissions(identity).Count(item => permissionCodes.Contains(item.Code)) == permissionCodes.Length;
        }

        public bool HasAnyPermissions(string identity, string[] permissionCodes)
        {
            CheckArgument.IsNotNullOrEmpty(identity, "登录名称");
            CheckArgument.IsNotNullOrEmpty(permissionCodes, "权限码");
            return this.GetOwnedPermissions(identity).Count(item => permissionCodes.Contains(item.Code)) > 0;
        }

        public bool IsInRole(string identity, string roleCode)
        {
            Check.Argument.IsNotNull(identity, "登录名称");
            Check.Argument.IsNotNull(roleCode, "角色代码");

            var user = this.GetDomainUserByIdentity(identity);
            MakesureUseExist(user);
            return user.Roles.Contains(roleCode);
        }

        public bool IsInAllRoles(string identity, params string[] roleCodes)
        {
            CheckArgument.IsNotNullOrEmpty(identity, "登录名称");
            CheckArgument.IsNotNullOrEmpty(roleCodes, "角色代码");

            var user = this.GetDomainUserByIdentity(identity);
            MakesureUseExist(user);

            var count = user.Roles.Count(item => roleCodes.Contains(item));
            return count == roleCodes.Length;
        }

        public IEnumerable<string> GetOwnedPermissionCodes(string identity)
        {
            //用户角色拥的权限和用户自身拥有权限的并集
            var user = this.GetDomainUserByIdentity(identity);
            return OwnedPermissionCodes(user);
        }

        #endregion

        #region 操作
        public User Save(User user, SysLoggerDto logger, bool isSetApproved = false)
        {
            Check.Argument.IsNotNull(user, "user");
            logger.IsCreate = user.IsNew();
            var savedUser = this.SaveUserInfo(user);
            if (isSetApproved)
            {
                this.Approve(savedUser.LoginName, logger, false);
            }
            LoggerService.Create(logger, user, "{0}用户", "用户{0}成功：ID【{1}】,登录名称【{2}】,用户类型【{3}】,显示名称【{4}】,内部员工号【{5}】", user.Id, user.LoginName, user.UserType.GetDescription(), user.DisplayName, user.EmployeeNo);
            return savedUser;
        }

        public User Create(CreateUserRequest request, SysLoggerDto logger)
        {
            Check.Argument.IsNotNull(request, "request");

            var user = new User()
            {
                LoginName = request.LoginName,
                Password = request.Password,
                Desc = request.Desc,
                DisplayName = request.DisplayName,
                Email = request.Email,
                EmployeeNo = request.EmployeeNo,
                ClientNo = request.ClientNo,
                IsApproved = request.IsApproved,
                UserType = request.IsCustomer ? UserType.Customer : UserType.Employee
            };
            this.Save(user, logger);
            if (request.IsApproved)
            {
                this.Approve(request.LoginName, logger, false);
            }
            else
            {
                this.DisApprove(request.LoginName, logger, false);
            }

            return user;
        }

        public void Approve(string identity, SysLoggerDto logger)
        {
            Approve(identity, logger, true);
        }

        public void DisApprove(string identity, SysLoggerDto logger)
        {
            DisApprove(identity, logger, true);
        }

        public void ResetPassword(string identity, SysLoggerDto logger)
        {
            CheckArgument.IsNotNullOrEmpty(identity, "登录名称");
            using (ITransactionCoordinator coordinator = TransactionCoordinatorFactory.Create(Context, _bus))
            {
                var user = this.GetDomainUserByIdentity(identity);
                MakesureUseExist(user);
                user.ResetPassword(true);
                this._repository.Update(user);
                if (user.UserType != Domain.Aggregates.UserAgg.UserType.InternalApi)
                {
                    this.RaiseUserInfoChangedEvent(user);
                }
                coordinator.Commit();
            }
            LoggerService.Create(SysLoggerType.Update, logger, "重置用户密码", "重置用户密码成功：用户名称【{1}】", identity);
        }

        public void ChangePassword(string identity, string oldPassword, string newPassword, SysLoggerDto logger)
        {
            CheckArgument.IsNotNullOrEmpty(identity, "登录名称");
            CheckArgument.IsNotNullOrEmpty(oldPassword, "原始密码");
            CheckArgument.IsNotNullOrEmpty(newPassword, "新密码");
            using (ITransactionCoordinator coordinator = TransactionCoordinatorFactory.Create(Context, _bus))
            {
                var user = this.GetDomainUserByIdentity(identity);
                MakesureUseExist(user);
                user.ChangePassword(oldPassword, newPassword);
                _repository.Update(user);
                //if (user.IsMatchedPasword(oldPassword))
                //{
                //    user.ChangePassword(newPassword);
                //    _userRepository.Update(user);
                //    Context.Commit();
                //}
                //else
                //{
                //    throw new PortalException(ErrorCodes.StringCodes.OldPassowrdNotMatch, ErrorMessage.OldPassowrdNotMatch);
                //}
                if (user.UserType != Domain.Aggregates.UserAgg.UserType.InternalApi)
                {
                    this.RaiseUserInfoChangedEvent(user);
                }
                coordinator.Commit();
            }
            LoggerService.Create(SysLoggerType.Update, logger, "更改用户密码", "更改用户密码成功：用户名称【{1}】", identity);
        }

        public void ChangePassword(string identity, string newPassword, SysLoggerDto logger)
        {
            CheckArgument.IsNotNullOrEmpty(identity, "登录名称");
            CheckArgument.IsNotNullOrEmpty(newPassword, "新密码");
            using (ITransactionCoordinator coordinator = TransactionCoordinatorFactory.Create(Context, _bus))
            {
                var user = this.GetDomainUserByIdentity(identity);
                MakesureUseExist(user);
                user.ChangePassword(newPassword);
                this._repository.Update(user);
                if (user.UserType != Domain.Aggregates.UserAgg.UserType.InternalApi)
                {
                    this.RaiseUserInfoChangedEvent(user);
                }
                coordinator.Commit();
            }
            LoggerService.Create(SysLoggerType.Update, logger, "更改用户密码", "更改用户密码(不验证旧密码)成功：用户名称【{1}】", identity);
        }
        #endregion

        #region 私有方法
        private IEnumerable<Permission> GetOwnedPermissions(string identity)
        {
            if (string.IsNullOrEmpty(identity))
            {
                return Enumerable.Empty<Permission>();
            }

            var ownedPerCodes = this.GetOwnedPermissionCodes(identity);
            if (ownedPerCodes.Any())
            {
                var pers = _permissionRepository.GetList(new PermissionCodeListSpecification(ownedPerCodes.ToArray()));
                return Array.ConvertAll(pers.ToArray(), item => DtoDomainMapper.ConvertToDto(item));
            }

            return Enumerable.Empty<Permission>();
        }

        private IEnumerable<string> OwnedPermissionCodes(DomainUser user)
        {
            if (user == null)
            {
                return Enumerable.Empty<string>();
            }
            var roles = _roleRepository.GetList(new RoleCodeListSpecification(user.Roles.ToArray()));
            var rolePersQuery = from r in roles
                                from p in r.Permissions
                                select p;

            //合并自身的权限
            return rolePersQuery.Union(user.Permissions).Distinct();
        }

        protected override ISpecification<DomainUser> ConvertToSpec(FindUserRequest request)
        {
            ISpecification<DomainUser> spec = null;
            if (request.IsApi)
            {
                spec = new UserTypeListSpecification(request.IsApi);
            }
            else
            {
                spec = new UserTypeSpecification(UserTypeMapper.MapToDomainUserType(request.UserType));
            }
            if (!string.IsNullOrEmpty(request.LoginName))
            {
                spec = spec.And(new UserContainLoginNameSpecification(request.LoginName));
            }
            if (!string.IsNullOrEmpty(request.EmployeeNo))
            {
                spec = spec.And(new UserContainEmployeeNoSpecification(request.EmployeeNo));
            }
            if (!string.IsNullOrEmpty(request.PermissionCode))
            {
                spec = spec.And(new UserContainPermissionSpecification(request.PermissionCode));
            }
            if (!string.IsNullOrEmpty(request.ClientNo))
            {
                spec = spec.And(new UserContainClientNoSpecification(request.ClientNo));
            }
            if (!string.IsNullOrEmpty(request.DisplayName))
            {
                spec = spec.And(new UserContainDisplayNameSpecification(request.DisplayName));
            }
            if (!string.IsNullOrEmpty(request.Email))
            {
                spec = spec.And(new UserContainEmailSpecification(request.Email));
            }
            if (!string.IsNullOrEmpty(request.Phone))
            {
                spec = spec.And(new UserContainPhoneSpecification(request.Phone));
            }
            if (request.Employees != null && request.Employees.Length > 0)
            {
                spec = spec.And(new EmployeeNoListSpecification(request.Employees));
            }
            if (request.FromCreatedDateTime.HasValue)
            {
                spec = spec.And(new CreatedOnStartSpecification(request.FromCreatedDateTime.Value));
            }
            if (request.ToCreatedDateTime.HasValue)
            {
                spec = spec.And(new CreatedOnEndSpecification(request.ToCreatedDateTime.Value));
            }
            return spec;
        }

        protected override User Map(DomainUser source)
        {
            return source == null ? null : DtoDomainMapper.ConvertToDto(source);
        }

        private void MakesureUseExist(DomainUser user)
        {
            CheckArgument.IsNotNull(user, "用户");
        }

        private DomainUser GetDomainUserByIdentity(string identity)
        {
            return _repository.Get(new UserLoginNameSpecification(identity).Or(new EmployeeNoSpecification(identity)));
        }

        private void RaiseUserInfoChangedEvent(DomainUser user)
        {
            this._bus.Publish(new UserInfoChangedEvent(user.Id));
            //this._bus.Commit();
        }

        private User SaveUserInfo(User user)
        {
            if (user.IsNew())
            {
                using (ITransactionCoordinator coordinator = TransactionCoordinatorFactory.Create(Context, _bus))
                {
                    DomainUser domainUser;
                    bool isApi = user.UserType == UserType.InternalApi;
                    if (isApi)
                    {
                        domainUser = new DomainUser(user.LoginName, Domain.Aggregates.UserAgg.UserType.InternalApi);
                    }
                    else
                    {
                        domainUser = new DomainUser(user.LoginName, user.Password, UserTypeMapper.MapToDomainUserType(user.UserType));
                    }
                    domainUser.Desc = user.Desc;
                    domainUser.DisplayName = user.DisplayName;
                    domainUser.CreatedOn = user.CreatedOn;
                    domainUser.CreatedBy = user.CreatedBy;
                    domainUser.UpdatedOn = user.UpdatedOn;
                    domainUser.UpdatedBy = user.UpdatedBy;
                    domainUser.SetEmail(user.Email);
                    if (user.UserType == UserType.InternalApi || user.UserType == UserType.ExternalApi)
                    {
                        domainUser.SetPhone(user.Phone);
                    }
                    domainUser.SetEmployeeNo(user.EmployeeNo);
                    if (user.UserType == UserType.Customer)
                    {
                        domainUser.SetClientNo(user.ClientNo);
                    }
                    //if (isApi)
                    //{
                    //    domainUser.ResetPassword();
                    //}
                    this._repository.Add(domainUser);
                    user.Id = domainUser.Id;
                    coordinator.Commit();
                }
            }
            else
            {
                var domainUser = _repository.GetByKey(user.Id);
                domainUser.Desc = user.Desc;
                domainUser.DisplayName = user.DisplayName;
                domainUser.UpdatedOn = user.UpdatedOn;
                domainUser.UpdatedBy = user.UpdatedBy;
                domainUser.SetEmail(user.Email);
                domainUser.SetClientNo(user.ClientNo);
                if (user.UserType == UserType.InternalApi || user.UserType == UserType.ExternalApi)
                {
                    if (!string.IsNullOrEmpty(user.Password))
                    {
                        domainUser.ChangePassword(user.OldPassword, user.Password);
                    }
                    domainUser.SetPhone(user.Phone);
                }
                user.LoginName = domainUser.LoginName;
                this._repository.Update(domainUser);
                Context.Commit();
            }

            return user;
        }

        private void Approve(string identity, SysLoggerDto logger, bool createLogger)
        {
            CheckArgument.IsNotNullOrEmpty(identity, "登录名称");
            using (ITransactionCoordinator coordinator = TransactionCoordinatorFactory.Create(Context, _bus))
            {
                var user = this.GetDomainUserByIdentity(identity);
                MakesureUseExist(user);
                if (!user.IsApproved)
                {
                    user.Enable();
                    user.UpdatedOn = DateTime.Now;
                    user.UpdatedBy = logger.CreatedBy;
                    this._repository.Update(user);
                    this.RaiseUserInfoChangedEvent(user);
                    coordinator.Commit();
                }
            }
            if (createLogger)
            {
                LoggerService.Create(SysLoggerType.Update, logger, "启用用户账号", "启用用户账号成功：用户名称【{1}】", identity);
            }
        }

        private void DisApprove(string identity, SysLoggerDto logger, bool createLogger)
        {
            CheckArgument.IsNotNullOrEmpty(identity, "登录名称");
            using (ITransactionCoordinator coordinator = TransactionCoordinatorFactory.Create(Context, _bus))
            {
                var user = this.GetDomainUserByIdentity(identity);
                MakesureUseExist(user);
                if (user.IsApproved)
                {
                    user.Disable();
                    user.UpdatedOn = DateTime.Now;
                    user.UpdatedBy = logger.CreatedBy;
                    this._repository.Update(user);
                    this.RaiseUserInfoChangedEvent(user);
                    coordinator.Commit();
                }
            }
            if (createLogger)
            {
                LoggerService.Create(SysLoggerType.Update, logger, "停用用户账号", "停用用户账号成功：用户名称【{1}】", identity);
            }
        }
        /// <summary>
        /// 如果URL不为空或者有子节点都显示
        /// </summary>
        /// <param name="current"></param>
        /// <param name="menus"></param>
        /// <returns></returns>
        private bool IsShow(Menu current, IEnumerable<Menu> menus)
        {
            if (!string.IsNullOrEmpty(current.Url))
            {
                return true;
            }
            var childList = menus.Where(u => u.ParentId == current.Id);
            if (LengthUtility.IsNullOrEmpty(childList))
            {
                return false;
            }
            foreach (var menu in childList)
            {
                if (IsShow(menu, menus))
                {
                    return true;
                }
            }
            return false;
        }
        #endregion
    }
}
