using Portal.Applications.Services;
using Portal.Web.Admin.Core;
using Portal.Web.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Portal.Web.Admin.Controllers
{
    public class PersonalController : Controller
    {
        public IUserManagerService UserManagerService;
        public PersonalController(IUserManagerService userManagerService)
        {
            this.UserManagerService = userManagerService;
        }
        #region 修改密码
        public ActionResult Index()
        {
            return PartialView();
        }
        [HttpPost]
        public JsonResult SavePwd(string PwdOld, string PwdNew, string PwdAffirm)
        {
            ReturnModel<int> result = new ReturnModel<int>();
            if (!PwdNew.Equals(PwdAffirm))
            {
                result.Data = 0;
                result.ErrorMessage = "新密码不一致，请确认！";
            }
            else
            {
                result.Data = 1;
                UserManagerService.ChangePassword(Core.PageUtility.CurrentUser.Identity.Name, PwdOld, PwdNew, PageUtility.GetLogger());
            }
            return new JsonResult() { Data = result };
        }
        #endregion
    }
}
