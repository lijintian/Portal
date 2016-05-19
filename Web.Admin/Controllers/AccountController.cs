/*
 EasyDDD
系统名称：  Portal门户系统管理平台
模块名称：  控制层
创建日期：  2015-07-29
作者：      吴勺良
内容摘要：  用户登陆控制类
*/

using System;

using Portal.Applications.Services;
using Portal.Dto;
using Portal.Infrastructure.Exceptions;
using Portal.Web.Admin.Core;
using Portal.Web.Admin.Model;
using Portal.SDK.Security;
using System.Web.Mvc;
using Portal.Web.Core;
using Portal.Web.Core.Common;
using Portal.Web.Core.Model;
using EasyDDD.Infrastructure.Crosscutting.Logging;

namespace Portal.Web.Admin.Controllers
{
    public class AccountController : Controller
    {
        private readonly ILoginMangerService _loginMangerService;
        private readonly IUserManagerService _userManagerService;
        public AccountController(ILoginMangerService loginMangerService, IUserManagerService userManagerService)
        {
            _loginMangerService = loginMangerService;
            _userManagerService = userManagerService;
        }

        [AllowAnonymous]
        public ActionResult Index()
        {
            return RedirectToAction("Login");
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult Login(string returnUrl, string amazonUkPromotion)
        {
            AmazonUkPromotion();
            
            var user = this.User as PortalPrincipal;
            if (user != null)
            {
                return this.RedirectToApp(user.Token, user.UserType, returnUrl);
            }

            return View();
        }

        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Login(LoginVM model, string returnUrl)
        {
            AmazonUkPromotion();

            if (!ModelState.IsValid)
            {
                LogicCheckHelper.MarkLoginFailPlus();
                return View();
            }
            if (model.CheckCode)
            {
                string msg = ValidateCodeHelper.CheckCode(model.validateKey, model.Code);
                if (!string.IsNullOrEmpty(msg))
                {
                    LogicCheckHelper.MarkLoginFailPlus();
                    ModelState.AddModelError("", msg);
                    return View();
                }
            }
            Dto.LoginInfo info = new Dto.LoginInfo();
            info.Identity = model.UserName.Trim();
            info.Password = model.Password;
            info.Source = "portal";
            try
            {
                var identity = _loginMangerService.Login(info, PageUtility.GetLogger());
                if (identity.IsLoginSuccessed())
                {
                    LogicCheckHelper.ClearLoginFailCookie();
                    switch (identity.UserType)
                    {
                        case UserType.InternalApi:
                            ModelState.AddModelError("", "登录失败：内部API账号不允许登陆.");
                            return View();
                            break;
                        case UserType.ExternalApi:
                            ModelState.AddModelError("", "登录失败：开发者账号不允许登陆.");
                            return View();
                            break;
                    }
                    //portal login in handle
                    var getUserResponse = this._userManagerService.GetPackageUserInfo(identity.LoginName);
                    if (getUserResponse.IsValid())
                    {
                        PortalAuthenticationHelper.LoginedIn(new UserPackageInfo(getUserResponse.LoginName,
                            getUserResponse.DisplayName,
                            identity.Token,
                            (UserType)Enum.Parse(typeof(UserType), getUserResponse.UserType),
                            getUserResponse.RoleCodes,
                            getUserResponse.PermissionCodes));
                        return RedirectToApp(identity.Token, identity.UserType, returnUrl);
                    }
                }
                ModelState.AddModelError("", "登录失败,请联系管理员.");
            }
            catch (PortalException ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            catch (Exception ex)
            {
                Log<AccountController>.LogError("登录失败", ex);
                ModelState.AddModelError("", "登录失败, 麻烦稍后重试");
            }
            LogicCheckHelper.MarkLoginFailPlus();
            return View();
        }

        public ActionResult Logout(string identity, string returnUrl)
        {
            PortalAuthenticationHelper.LoginOut(returnUrl, () =>
            {
                if (!string.IsNullOrEmpty(identity))
                {
                    _loginMangerService.LoginOut(identity);
                }
            });

            return this.Content("redirecting...");
        }

        [AllowAnonymous]
        public string RefreshToken()
        {
            if (!PageUtility.IsAuthenticated)
            {
                return new ReturnModel<string>(true).ToJson();
            }
            var ftoken = System.Web.Helpers.AntiForgery.GetHtml().ToString();
            System.Text.RegularExpressions.Regex r = new System.Text.RegularExpressions.Regex(@"value=""([^""]+)");
            var m = r.Match(ftoken);
            var newToken = m.Groups[1].Value;
            return new ReturnModel<string>(newToken, "", false).ToJson();
        }

        private ActionResult RedirectToApp(string token, UserType userType, string returnUrl)
        {
            var redirectUrl = AppSettingHelper.Get(AppSettingKey.Login_DEFAULT_Client_URL);
            //!CheckUtility.IsUrl(returnUrl) || 
            if (string.IsNullOrEmpty(returnUrl) || PortalAuthenticationHelper.IsAccessingPortalHomeOrLoginPage(returnUrl))
            {
                if (userType == UserType.Customer)
                {
                    redirectUrl = AppSettingHelper.Get(AppSettingKey.Login_DEFAULT_Client_URL);
                }
                else if (userType == UserType.Employee)
                {
                    redirectUrl = AppSettingHelper.Get(AppSettingKey.Login_DEFAULT_Store_URL);
                }
            }
            else
            {
                redirectUrl = returnUrl;
            }

            var redirectUri = new Uri(redirectUrl);
            var appendSign = string.IsNullOrEmpty(redirectUri.Query) ? "?" : "&";
            redirectUrl = string.Format("{0}{1}{2}={3}", redirectUrl, appendSign, PortalAuthenticationConfig.TokenUrlParameterName, token);

            return this.Redirect(redirectUrl);
        }

        /// <summary>
        /// amazon注册推广
        /// </summary>
        private void AmazonUkPromotion()
        {
            const string userRegisterUrl = "http://www.abc.com/User/Regesit.aspx";
            var amazonUkPromotionStr = Request.QueryString["AmazonUkPromotion"];
            ViewData["amazonUkPromotion"] = string.IsNullOrEmpty(amazonUkPromotionStr)
                ? userRegisterUrl
                : string.Format("{0}?AmazonUkPromotion={1}", userRegisterUrl, amazonUkPromotionStr);
        }
    }
}
