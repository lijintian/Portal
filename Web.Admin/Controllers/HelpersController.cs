using Junxy.DeployVersionDisplay;
using System.Web.Mvc;

namespace Portal.Web.Admin.Controllers
{
    public class HelpersController : Controller
    {
        [AllowAnonymous]
        [ChildActionOnly]
        public ActionResult GetDeployVersionString()
        {
            return Content(string.Format("[LOGIN:{0}]", VersionHelpers.GetDeployVersionCommitHash()));
        }
    }
}
