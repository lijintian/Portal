using Portal.SDK.Security;
using System;
using System.Web.Http.Controllers;
using AuthorizeAttribute = System.Web.Http.AuthorizeAttribute;

namespace Portal.Web.Core.Extends
{
    /// <summary>
    /// 验证权限的Attribute
    /// </summary>

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class PermissionAuthorizationAttribute : AuthorizeAttribute
    {
        private readonly string[] _permissionCodes;

        private readonly PermissionLinkType _permissionLinkType;
        public PermissionAuthorizationAttribute(PermissionLinkType permissionLinkType, string[] permissionCode)
        {
            this._permissionLinkType = permissionLinkType;
            this._permissionCodes = permissionCode;
        }
        public PermissionAuthorizationAttribute(string permissionCode)
            : this(PermissionLinkType.Or, new string[] { permissionCode })
        {

        }
        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            if (actionContext == null)
            {
                throw new ArgumentException("actionContext");
            }

            if (this._permissionCodes != null && this._permissionCodes.Length > 0)
            {
                var principal = actionContext.ControllerContext.RequestContext.Principal as PortalPrincipal;
                if (principal == null)
                {
                    //this case should never happen.
                    return false;
                }
                if (this._permissionLinkType == PermissionLinkType.And)
                {
                    return principal.HasAllPermissions(this._permissionCodes);
                }
                else
                {
                    return principal.HasAnyPermission(this._permissionCodes);
                }
            }
            else
            {
                return base.IsAuthorized(actionContext);
            }
        }

    }

    public enum PermissionLinkType
    {
        And,
        Or
    }

}
