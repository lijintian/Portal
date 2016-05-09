using System.Net;
using System.Web.Mvc;

namespace Portal.OAuth.Controllers
{
    public class ControlController : Controller
    {
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
