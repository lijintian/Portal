using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Portal.SDK.Security;

namespace Portal.Mvc
{
    /// <summary>
    /// 角色检查, 如果没权限将重定向到配置文件指定的NoPermissionPage页
    /// </summary>
    public class RoleAuthorizationAttribute : PortalAuthorizeAttribute
    {
        private readonly LinkType _linkType;
        private readonly string[] _codes;
        public RoleAuthorizationAttribute(LinkType link, params string[] codes)
        {
            this._linkType = link;
            this._codes = codes;
        }

        public RoleAuthorizationAttribute(string code)
            : this(LinkType.Or, new string[] {code})
        {
            
        }

        protected override bool HasPermissions(HttpContextBase httpContext)
        {
            if (this._codes != null && this._codes.Length > 0)
            {
                var principal = httpContext.User as CK1Principal;
                if (principal == null)
                {
                    //this case should never happen.
                    return false;
                }

                return _linkType == LinkType.And
                    ? principal.IsInAllRole(this._codes)
                    : principal.IsInAnyRole(this._codes);
            }
            return true;
        }
    }
}
