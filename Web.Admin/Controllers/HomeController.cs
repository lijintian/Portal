/*
 EasyDDD
系统名称：  Portal门户系统管理平台
模块名称：  控制层
创建日期：  2015-07-29
作者：      黄亮
内容摘要：  首页
*/

using System;
using System.Web.Mvc;
using Portal.Applications.Services;
using Portal.SDK.Security;
using Portal.Web.Admin.Core;

namespace Portal.Web.Admin.Controllers
{
    public class HomeController : Controller
    {
        private readonly ITokenManagerService _tokenManagerService;
        public HomeController(ITokenManagerService tokenManagerService)
        {
            this._tokenManagerService = tokenManagerService;
        }
        public ActionResult Index()
        {
            var principal = this.User as CK1Principal;
            if (principal != null && principal.IsCustomer)
            {
                return this.Redirect(AppSettingHelper.Get(AppSettingKey.Login_DEFAULT_Client_URL));
            }
            string dUrl = Request.QueryString[CK1PortalAuthenticationConfig.PortalDefaultUrl];
            dUrl = string.IsNullOrEmpty(dUrl) ? "~/Home/Index2" : dUrl;
            ViewBag.DefaultUrl = dUrl;
            return View();
        }

        public ActionResult Index2()
        {
            return View();
        }

        public ActionResult DirectToken()
        {
            this._tokenManagerService.DirectGetAccessToken("7f4a5861-93a0-4380-b3b3-fbeb72a5e2e4", "J67", "http://www.wish.com/back", "Add_New_Order");
            return Content("success");
        }

        #region Elmah异常日志测试
        public ActionResult Test()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GenerateError(string error)
        {
            throw new ApplicationException(error);
        }
        #endregion
    }
}