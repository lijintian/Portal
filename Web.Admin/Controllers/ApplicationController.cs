/*
 EasyDDD
系统名称：  Portal门户系统管理平台
模块名称：  控制层
创建日期：  2015-07-29
作者：      吴勺良
内容摘要：  应用系统管理控制类
*/
using Portal.Applications.Services;
using Portal.Dto;
using Portal.Mvc;
using Portal.Web.Admin.Core;
using Portal.Web.Admin.Model.Application;
using Portal.Web.Core;
using Portal.Web.Core.Extends;
using Portal.Web.Core.Model;
using System.Web.Mvc;
using PermissionAuthorizationAttribute = Portal.Mvc.PermissionAuthorizationAttribute;

namespace Portal.Web.Admin.Controllers
{
    /// <summary>
    ///  应用系统管理
    /// </summary>

    [PermissionAuthorization(PermissionCodes.Application)]
    public class ApplicationController : Controller
    {
        public IApplicationManagerService ApplicationManagerService;
        public IUserManagerService UserManagerService;
        public ApplicationController(IApplicationManagerService appService, IUserManagerService userManagerService)
        {
            this.ApplicationManagerService = appService;
            this.UserManagerService = userManagerService;
        }
        /// <summary>
        /// 应用系统列表
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View(GetData());
        }
        /// <summary>
        /// 刷新应用系统列表
        /// </summary>
        /// <returns></returns>
        public ActionResult Partial()
        {
            return PartialView(GetData());
        }
        private ApplicationVM GetData()
        {
            ApplicationVM model = new ApplicationVM();
            model.list = this.ApplicationManagerService.GetAll();
            model.IsCanAdd = UserManagerService.HasPermission(PageUtility.CurrentUser.Identity.Name, PermissionCodes.Application_Add); //检查用户是否具有指定的权限
            model.IsCanEdit = UserManagerService.HasPermission(PageUtility.CurrentUser.Identity.Name, PermissionCodes.Application_Edit);
            return model;
        }
        /// <summary>
        /// 编辑页面
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [PermissionAuthorization(PermissionCodes.Application_Edit)]
        public ActionResult Edit(string id)
        {
            Application info = this.ApplicationManagerService.GetById(id);
            return PartialView(info);
        }
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        [PermissionAuthorization(LinkType.Or, new string[] { PermissionCodes.Application_Add, PermissionCodes.Application_Edit })]
        [JsonException]
        public JsonResult Save(Application info)
        {
            info.InitOperateInfo();
            this.ApplicationManagerService.Save(info, PageUtility.GetLogger());
            ReturnModel<int> result = new ReturnModel<int>() { Data = 1 };
            return new JsonResult() { Data = result };
        }
        /// <summary>
        /// 新建页面
        /// </summary>
        /// <returns></returns>
        [PermissionAuthorization(PermissionCodes.Application_Add)]
        public ActionResult Add()
        {
            Application info = new Application();
            return PartialView("Edit", info);
        }
    }
}
