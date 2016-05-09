using System.Web.Mvc;

namespace Portal.Web.Admin.Controllers
{
    public class ErrorController : Controller
    {
        public ActionResult Index()
        {
            return View("~/Views/Shared/Error.cshtml");
        }
    }
}
