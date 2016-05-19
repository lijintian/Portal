using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Portal.SDK.Security;

namespace Portal.Mvc
{
    /// <summary>
    /// 权限检查,如果没权限将重定向到配置文件指定的NoPermissionPage页
    /// </summary>
    public class PermissionAuthorizationAttribute : PortalAuthorizeAttribute
    {
        private readonly LinkType _linkType;
        private readonly string[] _codes;

        public PermissionAuthorizationAttribute(LinkType link, params string[] codes)
        {
            this._linkType = link;
            this._codes = codes;
        }

        public PermissionAuthorizationAttribute(string permissionCode)
            : this(LinkType.Or, new string[] { permissionCode })
        {

        }

        protected override bool HasPermissions(HttpContextBase httpContext)
        {
            if (this._codes != null && this._codes.Length > 0)
            {
                var principal = httpContext.User as PortalPrincipal;
                if (principal == null)
                {
                    //this case should never happen.
                    return false;
                }

                return _linkType == LinkType.And
                    ? principal.HasAllPermissions(this._codes)
                    : principal.HasAnyPermission(this._codes);
            }
            return true;
        }
    }
}
