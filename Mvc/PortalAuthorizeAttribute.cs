using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Web;
using System.Web.Mvc;

namespace Portal.Mvc
{
    /// <summary>
    /// 表示Portal授权检查基类
    /// </summary>
    public abstract class PortalAuthorizeAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (!this.HasPermissions(httpContext))
            {
                httpContext.Response.StatusCode = 403;
            }

            return base.AuthorizeCore(httpContext);
        }

        protected abstract bool HasPermissions(HttpContextBase httpContext);

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);

            if (filterContext.Result == null && filterContext.HttpContext.Response.StatusCode == 403)
            {
                var actionName = filterContext.ActionDescriptor.ActionName;
                var redirect = ConfigurationManager.AppSettings["NoPermissionPage"];

                var externalActionNames = ConfigurationManager.AppSettings["ExternalActionNames"];
                if (externalActionNames.Contains(actionName))
                {//portal外部显示的无权限页面
                    redirect = ConfigurationManager.AppSettings["ExternalNoPermissionPage"];
                }
                if (!string.IsNullOrWhiteSpace(redirect))
                {
                    filterContext.Result = new RedirectResult(redirect);
                }
            }
        }
    }
}
