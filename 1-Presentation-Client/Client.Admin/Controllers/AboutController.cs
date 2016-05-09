using System.Linq;
using System.Text;
using System.Web.Mvc;
using Portal.Client.Core;
using Portal.Client.Model;

namespace Portal.Client.Controllers
{
    public class AboutController : Controller
    {
        [AllowAnonymous]
        public ActionResult Index(string code)
        {
            code = string.IsNullOrEmpty(code) ? "UserCreate" : code;
            var list = MenuCodes.GetMenuList(code);
            string title = PageUtility.ProjectName;
            if (list != null)
            {
                StringBuilder builder = new StringBuilder();
                var current = list.FirstOrDefault(u => !string.IsNullOrEmpty(u.ClassName));
                if (current != null)
                {
                    builder.AppendFormat("> <a href=\"#\">{0}</a>", current.Name);
                    if (current.Childs != null)
                    {
                        var child = current.Childs.FirstOrDefault(u => !string.IsNullOrEmpty(u.ClassName));
                        if (child != null)
                        {
                            builder.AppendFormat("> <span class=\"page_r_serach_current\">{0}</span>", child.Name);
                            title = child.Name;
                        }
                    }
                    ViewBag.CurrentLocation = builder.ToString();
                }
            }
            ViewBag.MenuList = list;
            ViewBag.Title = title;
            return View(code);
        }
    }
}
