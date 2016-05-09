using Portal.Applications.Services;
using Portal.Dto;
using Portal.Dto.Request.Enum;
using Portal.Mvc;
using Portal.Web.Admin.Core;
using Portal.Web.Admin.Model;
using Portal.Web.Admin.Model.Role;
using Portal.Web.Core;
using Portal.Web.Core.Extends;
using Portal.Web.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using PermissionAuthorizationAttribute = Portal.Mvc.PermissionAuthorizationAttribute;

namespace Portal.Web.Admin.Controllers
{
    [PermissionAuthorization(PermissionCodes.Role)]
    public partial class RoleController : Controller
    {
        #region IOC

        public IRoleManagerService RoleManagerService;
        public IApplicationManagerService ams;
        public IPermissionManagerService pms;
        public IAuthorizationManagerService authms;

        public RoleController(IRoleManagerService roleService,
            IApplicationManagerService appService,
            IPermissionManagerService permissionService,
             IAuthorizationManagerService authService)
        {
            this.RoleManagerService = roleService;
            this.ams = appService;
            this.pms = permissionService;
            this.authms = authService;
        }

        #endregion

        #region C

        /// <summary>
        /// 创建角色
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        [PermissionAuthorization(PermissionCodes.Role_Add)]
        public ActionResult Create(String appId)
        {
            ViewData["AllApplication"] = GetAllApplication();
            ViewData["CurrentApplicitionId"] = appId;
            Role info = new Role();
            return PartialView("Edit", info);
        }

        #endregion

        #region R

        #region 获取角色数据

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        private IEnumerable<Role> GetList(Role parameter)
        {
            IEnumerable<Role> list = RoleManagerService.GetByApplicationId(parameter.ApplicationId);
            return list;
        }

        /// <summary>
        /// 获取单行记录
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        private Role GetModel(string roleCode)
        {
            return RoleManagerService.GetByCode(roleCode);
        }

        #endregion

        #region 角色列表
        /// <summary>
        /// Index
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            IEnumerable<Application> allApp = GetAllApplication();
            ViewData["AllApplication"] = allApp;
            Role role = new Role();
            if (allApp.Count() > 0)
            {
                role.ApplicationId = allApp.First().Id;
            }
            ViewBag.BatchImportSource = GetBatchImportHtml();
            return View(GetList(role));
        }


        /// <summary>
        /// 角色列表
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public ActionResult ListPartial(Role parameter)
        {
            ViewData["AllApplication"] = GetAllApplication();
            return PartialView(GetList(parameter));
        }

        #endregion

        #region 获取所有应用系统
        private IEnumerable<Application> GetAllApplication()
        {
            IEnumerable<Application> list = ams.GetAll();
            return list;
        }

        #endregion

        #region 角色权限
        /// <summary>
        /// 角色权限设置
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        [PermissionAuthorization(PermissionCodes.Set_Role_Permission)]
        public ActionResult Permission(string roleCode)
        {
            #region 获取当前角色
            Role currentRole = GetModel(roleCode);
            ViewData["CurrentRole"] = currentRole;
            #endregion

            #region 获取当前系统所有的页面权限

            FindPermissionRequest request = new FindPermissionRequest();
            request.ApplicationId = currentRole.ApplicationId;
            request.PageIndex = 1;
            request.PageSize = Int32.MaxValue;

            FindPermissionResponse<PagePermission> sysAllPagePermission = pms.FindPermissions<PagePermission>(request);
            ViewData["sysAllPagePermission"] = sysAllPagePermission.Data;
            #endregion

            #region 获取当前系统所有的api权限
            FindPermissionResponse<ApiPermission> sysAllApiPermission = pms.FindPermissions<ApiPermission>(request);
            ViewData["sysAllApiPermission"] = sysAllApiPermission.Data;

            #endregion

            #region 获取当前角色拥有的权限
            IEnumerable<Permission> rolePermission = RoleManagerService.GetRolePermissions(roleCode);
            return PartialView(rolePermission);
            #endregion
        }
        #endregion

        #endregion

        #region U

        /// <summary>
        /// 编辑角色
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        [PermissionAuthorization(PermissionCodes.Role_Edit)]
        public ActionResult Edit(string roleCode)
        {
            ViewData["AllApplication"] = GetAllApplication();

            Role info = GetModel(roleCode);
            return PartialView(info);
        }


        /// <summary>
        /// 新增编辑角色保存
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        [PermissionAuthorization(LinkType.Or, new string[] { PermissionCodes.Role_Add, PermissionCodes.Role_Edit })]
        [JsonException]
        public JsonResult Save(Role info)
        {
            info.InitOperateInfo();
            ReturnModel<int> result = new ReturnModel<int>();
            //新增保存前确认角色代码的唯一性
            if (!RoleManagerService.IsUniqueCode(info.Code) && string.IsNullOrEmpty(info.Id))
            {
                result.Status = false;
                result.ErrorMessage = "角色代码重复，请修改";
            }
            else
            {
                RoleManagerService.Save(info, PageUtility.GetLogger());
                result.Status = true;
                result.ErrorMessage = "保存角色成功";
            }


            return new JsonResult() { Data = result };
        }


        /// <summary>
        ///保存角色权限
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        [PermissionAuthorization(PermissionCodes.Set_Role_Permission)]
        [JsonException]
        public JsonResult SavePermission(RolePermission roleP)
        {
            IEnumerable<String> rolePermission = roleP.Permission == null ? null : roleP.Permission.Split(',').ToList();

            ReturnModel<int> result = new ReturnModel<int>();

            if (string.IsNullOrEmpty(roleP.Permission))
            {
                rolePermission = Enumerable.Empty<string>();
            }

            result.Status = true;
            result.ErrorMessage = "保存角色权限成功";
            authms.SavePermissionsToRole(roleP.RoleId, rolePermission, PageUtility.GetLogger());

            return new JsonResult() { Data = result };
        }
        #endregion

        #region D

        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public JsonResult Delete(Role parameter)
        {
            ReturnModel<int> result = new ReturnModel<int>() { Data = 1 };
            return new JsonResult() { Data = result };
        }

        #endregion

        #region 私有方法
        private string GetBatchImportHtml()
        {
            List<BatchImportSelect> list = new List<BatchImportSelect>
            {
                new BatchImportSelect(TemplateType.ImportRole, PermissionCodes.ImportRole),
                new BatchImportSelect(TemplateType.CreateRolePermission, PermissionCodes.CreateRolePermission),
                new BatchImportSelect(TemplateType.DeleteRolePermission, PermissionCodes.DeleteRolePermission)
            };
            return PageUtility.GetBatchImportHtml(list);
        }
        #endregion
    }
}
