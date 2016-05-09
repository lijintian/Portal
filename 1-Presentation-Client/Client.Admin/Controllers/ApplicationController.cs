/*
 EasyDDD
系统名称：  Portal门户系统管理平台
模块名称：  控制层
创建日期：  2015-07-29

内容摘要：  外部开发者应用管理
*/
using System.Web.Mvc;
using Portal.Applications.Services;
using Portal.Client.Core;
using Portal.Dto;
using Portal.Web.Core;
using Portal.Web.Core.Extends;
using Portal.Web.Core.Model;
using Portal.Mvc;

namespace Portal.Client.Controllers
{
    [Mvc.PermissionAuthorization(ClientPermissionCodes.ClientDeveloperApp)]
    public class ApplicationController : BaseController
    {
        #region 初始化
        private readonly IDeveloperAppManagerService _appService;
        private readonly IPermissionManagerService _perService;
        private readonly IApiPermissionGroupManagerService _GroupService;
        public ApplicationController(IDeveloperAppManagerService appService, IPermissionManagerService perService, IApiPermissionGroupManagerService groupService)
        {
            this._appService = appService;
            this._perService = perService;
            this._GroupService = groupService;
        }
        #endregion

        #region 页面
        public ActionResult Index(FindDeveloperAppRequest parameter)
        {
            parameter.IsUnDeleted = true;
            parameter.UserId = CurrentUser.Identity.Name;//只能查看用户自己登陆的应用
            parameter.IsExternal = true;
            if (!parameter.IsPostBack)
            {
                var info = ListUtility.GetDefaultSelect;
                ViewBag.TypeSource = EnumListUtility<DeveloperAppType>.Options(info);
                ViewBag.StateSource = EnumListUtility<SearchDeveloperAppState>.Options(info);
            }
            return ReturnView(_appService.Search(parameter), parameter, "List", "ApplicationPager");
        }

        /// <summary>
        /// 查看页
        /// </summary>
        [Mvc.PermissionAuthorization(ClientPermissionCodes.ClientDeveloperApp_View)]
        public ActionResult ViewList(string id)
        {
            var info = _appService.GetById(id);
            info.RequestPermssionDesc = _appService.GetOwnedPermissionCode(info);
            ViewBag.PermissionList = this._GroupService.GetOpenedGroupList(info);
            return PartialView(info);
        }

        /// <summary>
        /// 审核列表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Mvc.PermissionAuthorization(ClientPermissionCodes.ClientDeveloperApp_Audit)]
        public ActionResult Audit(string id)
        {
            var info = _appService.GetById(id);
            info.RequestPermssionDesc = _appService.GetOwnedPermissionCode(info);
            return PartialView(info);
        }

        /// <summary>
        /// 修改页
        /// </summary>
        [Mvc.PermissionAuthorization(LinkType.Or, new string[] { ClientPermissionCodes.ClientDeveloperApp_Create, ClientPermissionCodes.ClientDeveloperApp_Update })]
        public ActionResult Detail(string id, string backPageId)
        {
            ViewBag.BackPageId = backPageId;
            DeveloperAppDto info = string.IsNullOrEmpty(id) ? new DeveloperAppDto() { State = DeveloperAppState.Developing } : _appService.GetById(id);
            ViewBag.TypeHtml = EnumListUtility<DeveloperAppType>.GetSource(SelectListType.SpanRadio, "cbxType", null, ((int)info.AppType).ToString());
            return PartialView(info);
        }

        /// <summary>
        /// 修改成功之后的审核查看页
        /// </summary>
        [Mvc.PermissionAuthorization(LinkType.Or, new string[] { ClientPermissionCodes.ClientDeveloperApp_Create, ClientPermissionCodes.ClientDeveloperApp_Update })]
        public ActionResult AuditView(string id, string backPageId)
        {
            ViewBag.BackPageId = backPageId;
            var info = _appService.GetById(id);
            ViewBag.FlowInfo = _appService.GetFlowInfo(info.State);
            return View(info);
        }
        #endregion

        #region 操作
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Mvc.PermissionAuthorization(ClientPermissionCodes.ClientDeveloperApp_Delete)]
        [JsonException]
        public string Delete(string id)
        {
            _appService.Delete(id, PageUtility.GetLogger());
            return ReturnJson("删除成功！", true);
        }
        /// <summary>
        /// 保存
        /// </summary>
        /// <returns></returns>
        [Mvc.PermissionAuthorization(LinkType.Or, new string[] { ClientPermissionCodes.ClientDeveloperApp_Create, ClientPermissionCodes.ClientDeveloperApp_Update })]
        [JsonException]
        public string Save(DeveloperAppDto app)
        {
            app.IsExternal = true;
            app.UserId = CurrentUser.Identity.Name;
            _appService.Save(app, PageUtility.GetLogger());
            return new ReturnModel<string>("保存成功！", true) { Data = app.Id }.ToJson();
        }
        /// <summary>
        /// 保存权限
        /// </summary>
        [JsonException]
        [Mvc.PermissionAuthorization(LinkType.Or, new string[] { ClientPermissionCodes.ClientDeveloperApp_Create, ClientPermissionCodes.ClientDeveloperApp_Update })]
        public string SubmitPermission(string id, string codes)
        {
            _appService.SaveRequestPermssions(id, codes, PageUtility.GetLogger());
            return ReturnJson("权限设置成功！", true);
        }
        /// <summary>
        /// 提交审核
        /// </summary>
        [Mvc.PermissionAuthorization(ClientPermissionCodes.ClientDeveloperApp_Audit)]
        [JsonException]
        public string SubmitAudit(string id)
        {
            _appService.SumitToApprove(id, PageUtility.GetLogger());
            return ReturnJson("提交审核成功！", true);
        }
        #endregion

        #region 私有方法
        #endregion
    }
}
