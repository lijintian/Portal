/*
 EasyDDD
系统名称：  Portal门户系统管理平台
模块名称：  控制层
创建日期：  2015-11-19

内容摘要：  外部开发者应用管理
*/

using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Portal.Applications.Services;
using Portal.Dto;
using Portal.Dto.Client;
using Portal.Web.Admin.Core;
using Portal.Web.Core;
using Portal.Web.Core.Extends;
using Portal.Web.Core.Model;

namespace Portal.Web.Admin.Controllers
{
    [PermissionAuthorization(PermissionCodes.DeveloperApp)]
    public class DeveloperAppController : BaseController
    {
        #region 初始化
        private readonly IDeveloperAppManagerService _appService;
        private readonly IPermissionManagerService _perService;
        private readonly IUserManagerService _userService;
        public DeveloperAppController(IDeveloperAppManagerService appService, IPermissionManagerService perService, IUserManagerService userService)
        {
            this._appService = appService;
            this._perService = perService;
            this._userService = userService;
        }
        #endregion

        #region 页面
        public ActionResult Index(FindDeveloperAppRequest parameter)
        {
            parameter.IsUnDeleted = true;
            if (!parameter.IsPostBack)
            {
                ViewBag.StateSource = EnumSource<SearchDeveloperAppState>.Options(true);
                ViewBag.UserSource = ListSource.Options(GetUserList(), true);
            }
            return ReturnView(_appService.Search(parameter), parameter, "List", "ApplicationPager");
        }

        /// <summary>
        /// 查看页
        /// </summary>
        [PermissionAuthorization(PermissionCodes.DeveloperApp_View)]
        public ActionResult View(string id)
        {
            var info = _appService.GetAppPermssionsGroupById(id);
            return PartialView(info);
        }

        /// <summary>
        /// 编辑页
        /// </summary>
        [PermissionAuthorization(PermissionCodes.DeveloperApp_Update)]
        public ActionResult Detail(string id)
        {
            var info = _appService.GetAppPermssionsGroupById(id);
            ViewBag.TypeHtml = EnumListUtility<DeveloperAppType>.GetSource(SelectListType.SpanRadio, "cbxType", null, ((int)info.AppType).ToString());
            return PartialView(info);
        }

        /// <summary>
        /// 审核列表
        /// </summary>
        /// <param name="id"></param>
        [PermissionAuthorization(PermissionCodes.DeveloperApp_Audit)]
        public ActionResult Audit(string id)
        {
            var info = _appService.GetAppPermssionsGroupById(id);
            return PartialView(info);
        }
        #endregion

        #region 操作
        /// <summary>
        /// 更新应用信息
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        [PermissionAuthorization(PermissionCodes.DeveloperApp_Update)]
        [JsonException]
        public string Save(DeveloperAppDto app)
        {
            //app.IsExternal = true;
            app.UserId = CurrentUser.Identity.Name;
            _appService.Update(app, PageUtility.GetLogger());
            return ReturnJson();
        }
        /// <summary>
        /// 审核通过
        /// </summary>
        [PermissionAuthorization(PermissionCodes.DeveloperApp_Audit)]
        [JsonException]
        public string Approve(DeveloperAppSubmissionContext request)
        {
            request.Manipulator = CurrentUser.Identity.Name;
            _appService.Approve(request,PageUtility.GetLogger());
            return ReturnJson("审核通过！", true);
        }
        /// <summary>
        /// 驳回审请
        /// </summary>
        [PermissionAuthorization(PermissionCodes.DeveloperApp_Audit)]
        [JsonException]
        public string Reject(DeveloperAppSubmissionContext request)
        {
            request.Manipulator = CurrentUser.Identity.Name;
            _appService.Reject(request, PageUtility.GetLogger());
            return ReturnJson("驳回成功！", true);
        }
        #endregion

        #region 私有方法
        /// <summary>
        /// 获取内部和外部API账号
        /// </summary>
        /// <returns></returns>
        public List<EnumModel> GetUserList()
        {
            var list = _userService.GetApiUserList();
            return list == null ? null : list.Select(u => new EnumModel(u.LoginName, u.LoginName)).ToList();
        }
        #endregion
    }
}
