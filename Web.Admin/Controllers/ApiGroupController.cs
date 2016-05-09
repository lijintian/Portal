using Portal.Applications.Services;
using Portal.Dto;
using Portal.Mvc;
using Portal.Web.Admin.Core;
using Portal.Web.Admin.Model.ApiPermissionGroup;
using Portal.Web.Core;
using Portal.Web.Core.Extends;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using PermissionAuthorizationAttribute = Portal.Mvc.PermissionAuthorizationAttribute;

namespace Portal.Web.Admin.Controllers
{
    /// <summary>
    /// API权限分组
    /// </summary>
    [PermissionAuthorization(PermissionCodes.ApiPermissionGroup)]
    public class ApiGroupController : BaseController
    {
        #region 初始化
        private readonly IApplicationManagerService _appService;
        private readonly IApiPermissionGroupManagerService _apiGroupService;
        private readonly IPermissionManagerService _perService;
        private readonly IAuthorizationManagerService _authService;
        public ApiGroupController(IApplicationManagerService appService, IApiPermissionGroupManagerService roleService,
            IPermissionManagerService permissionService, IAuthorizationManagerService authService)
        {
            this._appService = appService;
            this._apiGroupService = roleService;
            this._perService = permissionService;
            this._authService = authService;
        }
        #endregion

        #region 页面
        public ActionResult Index(FindApiPermissionGroupRequest request)
        {
            var list = _apiGroupService.GetList(request);
            return ReturnView(list, request.IsPostBack, "List");
        }
        #endregion

        #region 弹出层
        /// <summary>
        /// 创建API权限分组
        /// </summary>
        /// <returns></returns>
        [PermissionAuthorization(PermissionCodes.ApiPermissionGroup_Add)]
        public ActionResult Create()
        {
            ApiPermissionGroup info = new ApiPermissionGroup();
            return PartialView("Edit", info);
        }
        [PermissionAuthorization(PermissionCodes.ApiPermissionGroup_Edit)]
        public ActionResult Edit(string code)
        {
            ApiPermissionGroup info = GetModel(code);
            return PartialView("Edit", info);
        }

        /// <summary>
        /// 角色权限设置
        /// </summary>
        /// <returns></returns>
        [PermissionAuthorization(LinkType.Or, new string[] { PermissionCodes.Set_ApiPermissionGroup_Permission, PermissionCodes.View_ApiPermissionGroup_Permission })]
        public ActionResult Permission(string code)
        {
            GroupApiPermission info = new GroupApiPermission() { Code = code };
            #region 获取所有系统
            IEnumerable<Application> allApp = _appService.GetAll();
            ViewData["AllApplication"] = allApp;
            #endregion

            #region 获取当前用户所有角色对应的权限
            IEnumerable<Permission> rolePermission = _apiGroupService.GetApiPermissionGroupPermissions(code);
            info.PermissionCode = rolePermission != null && rolePermission.Any() ? string.Format(",{0},", string.Join(",", rolePermission.Select(u => u.Code))) : string.Empty;
            #endregion

            ViewBag.Permissions = info;
            return PartialView(allApp);
        }

        #endregion

        #region 操作
        /// <summary>
        /// 新增编辑API权限分组保存
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        [PermissionAuthorization(LinkType.Or, new string[] { PermissionCodes.ApiPermissionGroup_Add, PermissionCodes.ApiPermissionGroup_Edit })]
        [JsonException]
        public string Save(ApiPermissionGroup info)
        {
            info.InitOperateInfo();
            //新增保存前确认API权限分组代码的唯一性
            if (string.IsNullOrEmpty(info.Id) && !_apiGroupService.IsUniqueCode(info.Code))
            {
                return ReturnJson("保存失败，已存在相同的分组编码！");
            }
            else
            {
                _apiGroupService.Save(info, PageUtility.GetLogger());
                return ReturnJson("保存API权限分组成功", true);
            }
        }

        /// <summary>
        ///保存API权限分组权限
        /// </summary>
        /// <returns></returns>
        [PermissionAuthorization(PermissionCodes.Set_ApiPermissionGroup_Permission)]
        [JsonException]
        public string SavePermission(GroupApiPermission parameter)
        {
            IEnumerable<string> groupPermission = parameter.PermissionCode == null ? null : parameter.PermissionCode.Trim(',').Split(',').ToList();
            if (string.IsNullOrEmpty(parameter.PermissionCode) || string.IsNullOrEmpty(parameter.PermissionCode.Trim(',')))
            {
                groupPermission = Enumerable.Empty<string>();
            }
            _authService.SavePermissionsToApiGroup(parameter.Code, groupPermission, PageUtility.GetLogger());
            return ReturnJson("保存API权限分组权限成功", true);
        }
        #endregion

        #region 私有方法
        /// <summary>
        /// 获取单行记录
        /// </summary>
        /// <returns></returns>
        private ApiPermissionGroup GetModel(string code)
        {
            return _apiGroupService.GetByCode(code);
        }
        #endregion
    }
}
