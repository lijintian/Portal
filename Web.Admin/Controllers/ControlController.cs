using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Portal.Applications.Services;
using Portal.Dto;
using Portal.Web.Admin.Core;
using Portal.Web.Admin.Model;
using Portal.Web.Core;

namespace Portal.Web.Admin.Controllers
{
    public class ControlController : Controller
    {
        #region 初始化
        private IUserManagerService Service;
        public ControlController(IUserManagerService userService)
        {
            this.Service = userService;
        }
        #endregion

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

        #region 系统菜单用户控件
        /// <summary>
        /// 系统菜单用户控件
        /// </summary>
        /// <returns></returns>
        public ActionResult SysMenuControl()
        {
            Menu[] list = Service.GetUserMenus(HostUtility.Config.ApplicationId, PageUtility.CurrentUser.Identity.Name).ToArray();
            NavMenuInfo[] menus = GetNavMenuList(list);
            return PartialView(menus);
        }
        #endregion

        #region 其他页面
        /// <summary>
        /// 无权限提示页面
        /// </summary>
        /// <returns></returns>
        public ActionResult UnPermission()
        {
            return View();
        }

        /// <summary>
        /// 外部没权限提示页面
        /// </summary>
        /// <returns></returns>
        public ActionResult ExternalUnPermission()
        {
            return View();
        }


        public ActionResult NotFound()
        {
            return View("~/Views/Control/Page404.cshtml");
        }
        public ActionResult ServerError()
        {
            Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            return View("~/Views/Shared/Error.cshtml");
        }
        #endregion

        #region 私有方法
        /// <summary>
        /// 获取导航菜单
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        private NavMenuInfo[] GetNavMenuList(Menu[] list)
        {
            if (list.IsNullOrEmpty()) return null;
            string[] classList = new[] { "fa-desktop", "fa-dashboard", "fa-list-alt", "fa-th" };
            int length = classList.Length;
            MenuInfo[] firstList = list.Where(u => u.IsFirstParent()).OrderBy(u => u.Order).Select(u => new MenuInfo(u)).ToArray();
            if (firstList.IsNullOrEmpty()) return null;
            List<NavMenuInfo> result = new List<NavMenuInfo>();
            int i = 0;
            foreach (var item in firstList)
            {
                var navItem = new NavMenuInfo(item);
                GetChildList(navItem, list);
                if (navItem.ChildCount <= 0)
                {
                    continue;
                }
                navItem.Current.ClassName = classList[i % length];
                result.Add(navItem);
                i++;
            }
            return result.ToArray();
        }

        private void GetChildList(NavMenuInfo menu, Menu[] list)
        {
            List<MenuInfo> childList = list.Where(u => u.ParentId == menu.Current.Id).OrderBy(u => u.Order).Select(u => new MenuInfo(u)).ToList();
            if (childList.IsNullOrEmpty())
            {
                return;
            }
            menu.ChildCount = childList.Count;
            menu.Child = new List<NavMenuInfo>();
            foreach (var menuInfo in childList)
            {
                var item = new NavMenuInfo(menuInfo);
                GetChildList(item, list);
                menu.Child.Add(item);
            }
        }
        #endregion
    }
}
