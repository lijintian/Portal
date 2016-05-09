using System;
using System.Web.Mvc;

namespace Portal.Client.Controllers
{
    public class HomeController : Controller
    {
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View();
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
