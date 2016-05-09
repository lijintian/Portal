using System;
using System.Web.Mvc;

using Portal.Applications.Services;
using Portal.Client.Core;
using Portal.Client.Model;
using Portal.Dto;
using Portal.Infrastructure.Exceptions;
using Portal.SDK.Security;
using Portal.Web.Core;
using EasyDDD.Infrastructure.Crosscutting.Logging;

namespace Portal.Client.Controllers
{
    public class AccountController : Controller
    {
        #region 初始化
        private readonly ILoginMangerService _loginMangerService;
        private readonly IUserManagerService _userManagerService;
        public AccountController(ILoginMangerService loginMangerService, IUserManagerService userManagerService)
        {
            _loginMangerService = loginMangerService;
            _userManagerService = userManagerService;
        }
        #endregion

        #region 页面
        [AllowAnonymous]
        public ActionResult Index()
        {
            return RedirectToAction("Login");
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult Login(string returnUrl, string amazonUkPromotion)
        {
            var user = this.User as CK1Principal;
            if (user != null)
            {
                return this.RedirectToApp(user.Token, returnUrl);
            }
            return View();
        }

        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Login(LoginVM model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (Session[model.validateKey] == null)
            {
                ModelState.AddModelError("", "验证码已过期！");
                return View();
            }
            string msg = ValidateCodeHelper.CheckCode(model.validateKey, model.Code);
            if (!string.IsNullOrEmpty(msg))
            {
                ModelState.AddModelError("", msg);
                return View();
            }
            Dto.LoginInfo info = new Dto.LoginInfo();
            info.Identity = model.LoginUserName.Trim();
            info.Password = model.LoginPassword;
            info.Source = "appClient";
            try
            {
                var identity = _loginMangerService.Login(info, PageUtility.GetLogger());
                if (identity.IsLoginSuccessed())
                {
                    if (identity.UserType != UserType.ExternalApi)
                    {
                        ModelState.AddModelError("", "登录失败：非开发者账号不允许登陆.");
                        return View();
                    }
                    //portal login in handle
                    var getUserResponse = this._userManagerService.GetPackageUserInfo(identity.LoginName);
                    if (getUserResponse.IsValid())
                    {
                        Ck1PortalAuthenticationHelper.LoginedIn(new UserPackageInfo(getUserResponse.LoginName,
                            getUserResponse.DisplayName,
                            identity.Token,
                            (UserType)Enum.Parse(typeof(UserType), getUserResponse.UserType),
                            getUserResponse.RoleCodes,
                            getUserResponse.PermissionCodes));
                        return RedirectToApp(identity.Token, returnUrl);
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
            return View();
        }

        #endregion

        #region 操作
        [AllowAnonymous]
        public ActionResult Logout(string identity, string returnUrl)
        {
            if (string.IsNullOrEmpty(returnUrl))
            {
                returnUrl = "/";
            }
            Ck1PortalAuthenticationHelper.LoginOut(returnUrl, () =>
            {
                if (!string.IsNullOrEmpty(identity))
                {
                    _loginMangerService.LoginOut(identity);
                }
            });
            return this.Content("redirecting...");
        }
        #endregion

        #region 私有方法
        private ActionResult RedirectToApp(string token, string returnUrl)
        {
            if (string.IsNullOrEmpty(returnUrl))
            {
                returnUrl = "/";
            }
            //var redirectUri = new Uri(returnUrl);
            //var appendSign = string.IsNullOrEmpty(redirectUri.Query) ? "?" : "&";
            var appendSign = returnUrl.IndexOf("?") <= 0 ? "?" : "&";
            returnUrl = string.Format("{0}{1}{2}={3}", returnUrl, appendSign, CK1PortalAuthenticationConfig.TokenUrlParameterName, token);
            return this.Redirect(returnUrl);
        }
        #endregion
    }
}
