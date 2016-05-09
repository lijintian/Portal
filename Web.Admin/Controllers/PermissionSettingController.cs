using Portal.Applications.Services;
using Portal.Dto;
using Portal.Mvc;
using Portal.Web.Admin.Core;
using Portal.Web.Admin.Model.PermissionSetting;
using Portal.Web.Core;
using Portal.Web.Core.Extends;
using Portal.Web.Core.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using PermissionAuthorizationAttribute = Portal.Mvc.PermissionAuthorizationAttribute;

namespace Portal.Web.Admin.Controllers
{
    [PermissionAuthorization(LinkType.Or, new string[] { PermissionCodes.User_SetRole, PermissionCodes.User_SetPermission, PermissionCodes.Customer_SetRole, PermissionCodes.Customer_SetPermission, PermissionCodes.APIUser_SetRole, PermissionCodes.APIUser_SetPermission })]
    public partial class PermissionSettingController : BaseController
    {
        #region IOC

        public IAuthorizationManagerService AuthorizationManagerService;
        public IUserManagerService ums;
        public IApplicationManagerService ams;
        public IRoleManagerService rms;
        public IPermissionManagerService pms;

        public PermissionSettingController(IAuthorizationManagerService authService,
            IUserManagerService userService,
            IApplicationManagerService appService,
            IRoleManagerService roleService,
            IPermissionManagerService permissionService)
        {
            this.AuthorizationManagerService = authService;
            this.ums = userService;
            this.ams = appService;
            this.rms = roleService;
            this.pms = permissionService;
        }

        #endregion

        #region R

        #region 用户权限外部跳转
        [PermissionAuthorization(PermissionCodes.User_SetPermission)]
        public ActionResult UserPermissionSettingExternal(String loginName)
        {
            #region 获取当前用户
            User currentUser = ums.GetByIdentity(loginName);
            ViewData["CurrentUser"] = currentUser;
            #endregion

            #region 如果非员工
            ViewData["IsEmployee"] = true;
            if (currentUser != null && currentUser.UserType != UserType.Employee)
            {
                ViewData["IsEmployee"] = false;
            }
            #endregion

            return View(currentUser);
        }
        #endregion

        #region 克隆某用户权限到另一个用户
        [PermissionAuthorization(LinkType.Or, new string[] { PermissionCodes.User_SetPermission, PermissionCodes.Customer_SetPermission, PermissionCodes.APIUser_SetPermission })]
        public ActionResult ClonePermissionSetting()
        {
            ClonePermission sp = new ClonePermission();
            sp.ToUserLoginName = Request["loginName"];
            sp.userType = Convert.ToInt32(Request["userType"]);
            return PartialView(sp);
        }
        #endregion

        #region 业务系统客户权限设置专用页面
        [PermissionAuthorization(PermissionCodes.Customer_SetPermission)]
        public ActionResult CustomerPermissionSettingExternal(String loginName)
        {
            #region 获取当前用户
            User currentUser = ums.GetByIdentity(loginName);
            ViewData["CurrentUser"] = currentUser;
            #endregion

            #region 如果非客户
            ViewData["IsCustomer"] = true;
            if (currentUser != null && currentUser.UserType != UserType.Customer)
            {
                ViewData["IsCustomer"] = false;
            }
            #endregion

            return View(currentUser);
        }
        #endregion

        #region Asyn

        #region 用户权限
        /// <summary>
        /// 异步加载用户权限
        /// </summary>
        /// <param name="loginName"></param>
        /// <returns></returns>
        [PermissionAuthorization(PermissionCodes.User_SetPermission)]
        public ActionResult AsynUserPermissionSetting(String loginName)
        {
            #region 获取当前用户
            User currentUser = ums.GetByIdentity(loginName);
            ViewData["CurrentUser"] = currentUser;
            #endregion

            #region 获取所有系统
            IEnumerable<Application> allApp = ams.GetAll();

            #region 如果用户类型为客户，只显示配置中过滤的系统的权限设置
            if (currentUser.UserType == UserType.Customer)
            {
                allApp = PermissionSettingFilter(allApp);
            }
            #endregion

            ViewData["AllApplication"] = allApp;
            #endregion

            #region 获取当前用户原页面权限
            IEnumerable<string> curUserPermissions = ums.GetOwnedPermissionCodes(currentUser.LoginName).ToList();
            ViewData["curUserPermissions"] = curUserPermissions.Any() ? string.Format(",{0},", string.Join(",", curUserPermissions)) : ",";
            #endregion

            #region 获取当前用户所有角色对应的权限
            ViewData["currentUerRolePermissions"] = GetUserRolePermissions(currentUser);
            #endregion

            return PartialView(allApp);

        }

        /// <summary>
        /// 异步加载系统页面权限
        /// </summary>
        /// <param name="applicationId"></param>
        /// <returns></returns>
        [PermissionAuthorization(LinkType.Or, new string[] { PermissionCodes.User_SetPermission, PermissionCodes.Customer_SetPermission })]
        public ActionResult AsynPagePermission(FindPermissionRequest request)
        {
            #region 获取当前系统所有的页面权限
            IEnumerable<PagePermission> sysAllPagePermission = pms.GetList<PagePermission>(request);
            List<PagePermission> pagePermissions = sysAllPagePermission.ToList();
            #endregion

            return PartialView(pagePermissions);
        }
        #endregion

        #region 客户权限
        [PermissionAuthorization(PermissionCodes.Customer_SetPermission)]
        public ActionResult AsynCustomerPermissionSetting(String loginName)
        {
            #region 获取当前用户
            User currentUser = ums.GetByIdentity(loginName);
            ViewData["CurrentUser"] = currentUser;
            #endregion

            #region 获取所有系统
            IEnumerable<Application> allApp = ams.GetAll();

            #region 用户类型为客户，只显示配置中过滤的系统的权限设置
            allApp = PermissionSettingFilter(allApp);
            #endregion

            ViewData["AllApplication"] = allApp;
            #endregion

            #region 获取当前用户原页面权限
            IEnumerable<string> curUserPermissions = ums.GetOwnedPermissionCodes(currentUser.LoginName).ToList();
            ViewData["curUserPermissions"] = curUserPermissions.Any() ? string.Format(",{0},", string.Join(",", curUserPermissions)) : ",";
            #endregion

            #region 获取当前用户所有角色对应的权限
            ViewData["currentUerRolePermissions"] = GetUserRolePermissions(currentUser);
            #endregion

            return PartialView("AsynUserPermissionSetting", allApp);

        }

        #endregion

        #region Api帐号权限
        /// <summary>
        /// 异步加载API用户权限
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [PermissionAuthorization(PermissionCodes.APIUser_SetPermission)]
        public ActionResult AsynApiUserPermissionSetting(String loginName)
        {
            #region 获取当前用户
            User currentUser = ums.GetByIdentity(loginName);
            ViewData["CurrentUser"] = currentUser;
            #endregion

            #region 获取所有系统
            IEnumerable<Application> allApp = ams.GetAll();
            ViewData["AllApplication"] = allApp;
            #endregion

            #region 获取当前用户原API权限
            IEnumerable<string> curUserPermissions = ums.GetOwnedPermissionCodes(currentUser.LoginName).ToList();
            ViewData["curUserPermissions"] = curUserPermissions.Any() ? string.Format(",{0},", string.Join(",", curUserPermissions)) : ",";
            #endregion

            #region 获取当前用户所有角色对应的权限
            ViewData["currentUerRolePermissions"] = GetUserRolePermissions(currentUser);
            #endregion

            return PartialView(allApp);

        }
        /// <summary>
        /// 异步加载系统api权限
        /// </summary>
        /// <returns></returns>
        [PermissionAuthorization(PermissionCodes.APIUser_SetPermission)]
        public ActionResult AsynApiPermission(FindPermissionRequest request)
        {
            #region 获取当前系统所有的API权限
            IEnumerable<ApiPermission> sysAllPagePermission = pms.GetList<ApiPermission>(request);
            List<ApiPermission> apiPermissions = sysAllPagePermission.ToList();
            #endregion

            return PartialView(apiPermissions);
        }
        #endregion

        #region 用户角色
        /// <summary>
        /// 异步加载角色
        /// </summary>
        /// <returns></returns>
        [PermissionAuthorization(LinkType.Or, new string[] { PermissionCodes.User_SetRole, PermissionCodes.Customer_SetRole })]
        public ActionResult AsynUserRoleSetting(string loginName)
        {
            #region 获取当前用户
            User currentUser = ums.GetByIdentity(loginName);
            ViewData["CurrentUser"] = currentUser;
            #endregion

            #region 获取所有系统
            IEnumerable<Application> allApp = ams.GetAll();

            #region 如果用户类型为客户，只显示配置中过滤的系统的权限设置
            if (currentUser.UserType == UserType.Customer)
            {
                allApp = PermissionSettingFilter(allApp);
            }
            #endregion

            ViewData["AllApplication"] = allApp;
            #endregion

            #region 获取当前用户原角色
            IEnumerable<Role> currentUserRoles = ums.GetRoles(currentUser.LoginName);
            string roleCodes = string.Format(",{0},", string.Join(",", currentUserRoles.Select(u => u.Code)));
            ViewData["currentUserRoles"] = roleCodes;
            #endregion

            return PartialView(allApp);
        }

        /// <summary>
        /// 异步加载系统角色
        /// </summary>
        /// <param name="applicationId"></param>
        /// <returns></returns>
        [PermissionAuthorization(LinkType.Or, new string[] { PermissionCodes.User_SetRole, PermissionCodes.Customer_SetRole })]
        public ActionResult AsynRole(string applicationId)
        {
            List<Role> roles = rms.GetByApplicationId(applicationId).ToList();
            return PartialView(roles);
        }
        #endregion

        #region 客户设置权限filter

        private List<Application> PermissionSettingFilter(IEnumerable<Application> allApp)
        {
            #region 根据配置中过滤的系统
            List<Application> apps = new List<Application>();
            string[] appFilters = (ConfigurationManager.AppSettings["CustumerPermissionSettingFilter"] ?? "业务前端系统").Split(',');
            foreach (string appFilter in appFilters)
            {
                Application app = allApp.First(a => a.CnName == appFilter);
                if (app != null)
                {
                    apps.Add(app);
                }
            }
            return apps;
            #endregion
        }

        #endregion

        #region 获取用户所有角色所有权限
        private string GetUserRolePermissions(User user)
        {
            #region 获取当前用户原角色
            IEnumerable<Role> userRoles = ums.GetRoles(user.LoginName);
            #endregion

            #region 获取角色对应权限
            List<Permission> userRolePermissions = new List<Permission>();
            foreach (Role role in userRoles)
            {
                IEnumerable<Permission> rolePermissions = rms.GetRolePermissions(role.Code);
                userRolePermissions.AddRange(rolePermissions);
            }
            #endregion

            return string.Format(",{0},", string.Join(",", userRolePermissions.Select(u=>u.Code)));
        }

        #endregion

        #endregion

        #endregion

        #region U

        #region 保存用户角色
        /// <summary>
        ///保存用户角色
        /// </summary>
        /// <param name="userR"></param>
        /// <returns></returns>
        [PermissionAuthorization(LinkType.Or, new string[] { PermissionCodes.User_SetRole, PermissionCodes.Customer_SetRole, PermissionCodes.APIUser_SetRole })]
        [JsonException]
        public JsonResult SaveUserRole(UserRole userR)
        {
            IEnumerable<String> userRoleCodes = userR.RoleCode == null ? null : userR.RoleCode.Trim(',').Split(',').ToList();

            ReturnModel<int> result = new ReturnModel<int>();

            if (string.IsNullOrEmpty(userR.RoleCode.Trim(',')))
            {
                result.Status = false;
                result.ErrorMessage = "您没有选择角色，请选择";
                userRoleCodes = Enumerable.Empty<string>();
            }

            result.Status = true;
            result.ErrorMessage = "保存用户角色成功";
            AuthorizationManagerService.SaveUserToRoles(userR.UserId, userRoleCodes, PageUtility.GetLogger());


            return new JsonResult() { Data = result };
        }
        #endregion

        #region 保存(API)用户权限

        /// <summary>
        ///保存用户角色
        /// </summary>
        /// <param name="userP"></param>
        /// <returns></returns>
        [PermissionAuthorization(LinkType.Or, new string[] { PermissionCodes.User_SetPermission, PermissionCodes.Customer_SetPermission, PermissionCodes.APIUser_SetPermission })]
        [JsonException]
        public string SaveUserPermission(UserPermission userP)
        {
            IEnumerable<String> userPermissionCodes = userP.PermissionCode == null ? null : userP.PermissionCode.Trim(',').Split(',').ToList();
            if (string.IsNullOrEmpty(userP.PermissionCode.Trim(',')))
            {
                userPermissionCodes = Enumerable.Empty<string>();
            }
            AuthorizationManagerService.SaveUserOwnedPermissions(userP.UserId, userPermissionCodes, PageUtility.GetLogger());
            return ReturnJson("保存用户权限成功！", true);
        }
        #endregion

        #region 同步用户权限
        [PermissionAuthorization(LinkType.Or, new string[] { PermissionCodes.User_SetPermission, PermissionCodes.Customer_SetPermission, PermissionCodes.APIUser_SetPermission })]
        [JsonException]
        public string SaveClonePermission(ClonePermission cloneP)
        {
            #region 获取来源用户权限
            try
            {
                User fromUser = ums.GetByIdentity(cloneP.FromUserLoginName);
                if (fromUser == null)
                {
                    return ReturnJson("源用户不存在");
                }
            }
            catch
            {
                return ReturnJson("源用户不存在");
            }
            IEnumerable<string> fromUserPermissions = ums.GetOwnedPermissionCodes(cloneP.FromUserLoginName).ToList();
            #endregion

            #region 保存目标用户权限
            string toUserId;
            try
            {
                User toUser = ums.GetByIdentity(cloneP.ToUserLoginName);
                if (toUser == null)
                {
                    return ReturnJson("源用户不存在");
                }
                toUserId = toUser.Id;
            }
            catch
            {
                return ReturnJson("目标用户不存在");
            }

            AuthorizationManagerService.SaveUserOwnedPermissions(toUserId, fromUserPermissions, PageUtility.GetLogger());
            return ReturnJson("同步用户权限成功", true);
            #endregion
        }
        #endregion

        #endregion

        #region D



        #endregion

    }
}