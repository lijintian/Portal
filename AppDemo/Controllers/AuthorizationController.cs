using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AppDemo.Controllers
{
    public class AuthorizationController : Controller
    {
        //
        // GET: /Authorization/
        public ActionResult DoAuth()
        {
            return View();
        }

        public ActionResult GetCode(string code)
        {
            this.ViewBag.Code = code;
            return this.View();
        }
	}
}