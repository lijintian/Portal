using System.Net;
using System.Web.Mvc;
using Portal.Web.Core;

namespace Portal.Client.Controllers
{
    public class ControlController : Controller
    {
        #region 获取验证码
        [AllowAnonymous]
        public ActionResult GetValidateCode(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return PartialView();
            }
            ValidateCodeHelper vCode = new ValidateCodeHelper();
            string code = vCode.CreateValidateCode();
            Session[key] = code;
            byte[] bytes = vCode.CreateValidateGraphic(code);
            return File(bytes, @"image/jpeg");
        }
        #endregion

        #region 其他页面
        /// <summary>
        /// 无权限提示页面
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public ActionResult UnPermission()
        {
            return View();
        }

        /// <summary>
        /// 外部没权限提示页面
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public ActionResult ExternalUnPermission()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult NotFound()
        {
            return View("~/Views/Control/Page404.cshtml");
        }

        [AllowAnonymous]
        public ActionResult ServerError()
        {
            Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            return View("~/Views/Shared/Error.cshtml");
        }
        #endregion
    }
}
