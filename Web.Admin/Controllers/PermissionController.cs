/*
 EasyDDD
系统名称：  Portal门户系统管理平台
模块名称：  控制层
创建日期：  2015-07-29

内容摘要：  系统页面权限管理、API权限管理
*/
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Portal.Applications.Services;
using Portal.Dto;
using Portal.Mvc;
using Portal.Web.Admin.Core;
using Portal.Web.Core;
using Portal.Web.Core.Extends;
using Portal.Web.Core.Model;
using PermissionAuthorizationAttribute = Portal.Mvc.PermissionAuthorizationAttribute;

namespace Portal.Web.Admin.Controllers
{
    [PermissionAuthorization(LinkType.Or, new string[] { PermissionCodes.Permission, PermissionCodes.APIPermission })]
    public class PermissionController : BaseController
    {
        #region 初始化
        private IPermissionManagerService _service;
        private IApplicationManagerService _appService;
        public PermissionController(IPermissionManagerService service, IApplicationManagerService appService)
        {
            this._service = service;
            this._appService = appService;
        }
        #endregion

        #region 获取数据源
        /// <summary>
        /// 获取系统下拉框数据源
        /// </summary>
        /// <returns></returns>
        public SelectList GetApplicationList(string systemId = "")
        {
            var appList = _appService.GetAll();
            List<EnumModel> list = !appList.IsNullOrEmpty() ? appList.Select(u => new EnumModel(u.Id, u.CnName)).ToList() : null;
            return ListSource.Options(list, true, systemId);
        }
        #endregion

        #region 权限列表
        /// <summary>
        /// 系统页面权限列表
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        [PermissionAuthorization(PermissionCodes.Permission)]
        public ActionResult Index(FindPermissionRequest parameter)
        {
            if (!parameter.IsPostBack)
            {
                ViewBag.ApplicationList2 = GetApplicationList();
            }
            var source2 = _service.FindPermissions<PagePermission>(parameter);
            if (source2 != null)
            {
                if (source2.Data != null)
                {
                    foreach (var item in source2.Data)
                    {
                        item.FunctionPermissionSummary = !item.FunctionPermissions.IsNullOrEmpty() ? string.Join("&", item.FunctionPermissions.Select(u => string.Format("{0}【{1}】", u.Name, u.Code))) : "";
                    }
                }
            }
            return ReturnView(source2, parameter, "List", "PermissionPager");
        }
        /// <summary>
        /// API权限列表
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        [PermissionAuthorization(PermissionCodes.APIPermission)]
        public ActionResult API(FindPermissionRequest parameter)
        {
            if (!parameter.IsPostBack)
            {
                ViewBag.ApplicationList2 = GetApplicationList();
            }
            var source = _service.FindPermissions<ApiPermission>(parameter);
            return ReturnView(source, parameter, "APIList", "PermissionPager");
        }
        #endregion

        #region 权限信息编辑页面
        /// <summary>
        /// 权限信息编辑页面
        /// </summary>
        /// <param name="code"></param>
        /// <param name="applicationId">系统ID</param>
        /// <returns></returns>
        [PermissionAuthorization(LinkType.Or, new string[] { PermissionCodes.Permission_Add, PermissionCodes.Permission_Update })]
        public ActionResult Detail(string code, string applicationId)
        {
            PagePermission info = string.IsNullOrEmpty(code) ? new PagePermission() { ApplicationId = applicationId } : _service.GetByCode<PagePermission>(code);
            ViewBag.ApplicationList = GetApplicationList(info.ApplicationId);
            return PartialView(info);
        }
        /// <summary>
        /// 权限信息编辑页面
        /// </summary>
        /// <param name="code"></param>
        /// <param name="applicationId">系统ID</param>
        /// <returns></returns>
        [PermissionAuthorization(LinkType.Or, new string[] { PermissionCodes.APIPermission_Add, PermissionCodes.APIPermission_Update })]
        public ActionResult ApiDetail(string code, string applicationId)
        {
            ApiPermission info = string.IsNullOrEmpty(code) ? new ApiPermission() { ApplicationId = applicationId } : _service.GetByCode<ApiPermission>(code);
            ViewBag.ApplicationList = GetApplicationList(info.ApplicationId);
            return PartialView(info);
        }
        #endregion

        #region 操作权限信息
        /// <summary>
        /// 删除权限
        /// </summary>
        /// <returns></returns>
        [JsonException]
        public string Delete(string id)
        {
            _service.Remove(id);
            return ReturnJson();
        }

        /// <summary>
        /// 保存系统页面权限信息
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        [PermissionAuthorization(LinkType.Or, new string[] { PermissionCodes.Permission_Add, PermissionCodes.Permission_Update })]
        [JsonException]
        public string Save(PagePermission info)
        {
            if (string.IsNullOrEmpty(info.Id) && !_service.IsUniqueCode(info.Code))
            {
                return ReturnJson(string.Format("保存失败，权限码[{0}]已被使用！", info.Code));
            }
            if (!string.IsNullOrEmpty(info.FunctionPermissionSummary))
            {
                List<FunctionPermission> funcList = info.FunctionPermissionSummary.FromJson<List<FunctionPermission>>();
                funcList = funcList.Where(u => !string.IsNullOrEmpty(u.Name)).ToList();
                foreach (var item in funcList)
                {
                    if (funcList.Count(u => u.Code == item.Code) > 1)
                    {
                        return ReturnJson(string.Format("保存失败，权限码[{0}]提交了多次！", item.Code));
                    }
                    if (string.IsNullOrEmpty(item.Id) && !_service.IsUniqueCode(item.Code))
                    {
                        return ReturnJson(string.Format("保存失败，权限码[{0}]已被使用！", item.Code));
                    }
                    if (!string.IsNullOrEmpty(item.Name))
                    {
                        info.AddFuncPermission(item);
                    }
                }
            }
            info.InitOperateInfo();
            _service.Save(info, PageUtility.GetLogger());
            return ReturnJson();
        }
        /// <summary>
        /// 保存API权限信息
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        [PermissionAuthorization(LinkType.Or, new string[] { PermissionCodes.APIPermission_Add, PermissionCodes.APIPermission_Update })]
        [JsonException]
        public string ApiSave(ApiPermission info)
        {
            if (string.IsNullOrEmpty(info.Id) && !_service.IsUniqueCode(info.Code))
            {
                return ReturnJson("保存失败，权限码已被使用！");
            }
            info.InitOperateInfo();
            _service.Save(info, PageUtility.GetLogger());
            return ReturnJson();
        }
        #endregion
    }
}
