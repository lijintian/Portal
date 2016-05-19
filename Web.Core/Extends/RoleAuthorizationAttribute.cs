using System;
using System.Web.Http.Controllers;
using Portal.SDK.Security;
using AuthorizeAttribute = System.Web.Http.AuthorizeAttribute;

namespace Portal.Web.Core.Extends
{
    /// <summary>
    /// 验证角色的Attribute
    /// </summary>

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class RoleAuthorizationAttribute : AuthorizeAttribute
    {
        private readonly string[] _roleCodes;

        private readonly RoleLinkType _roleLinkType;
        public RoleAuthorizationAttribute(RoleLinkType roleLinkType, params string[] roleCode)
        {
            this._roleLinkType = roleLinkType;
            this._roleCodes = roleCode;
        }

        public RoleAuthorizationAttribute(string roleCode) :
            this(RoleLinkType.Or, new string[] {roleCode})
        {
            
        }

        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            if (actionContext == null)
            {
                throw new ArgumentException("actionContext");
            }

            if (this._roleCodes != null && this._roleCodes.Length > 0)
            {
                var principal = actionContext.ControllerContext.RequestContext.Principal as PortalPrincipal;
                if (principal == null)
                {
                    //this case should never happen.
                    return false;
                }
                if (this._roleLinkType == RoleLinkType.And)
                {
                    return principal.IsInAllRole(this._roleCodes);
                }
                else
                {
                    //or
                    return principal.IsInAnyRole(this._roleCodes);
                }
            }
            else
            {
                return base.IsAuthorized(actionContext);
            }
        }

    }

    public enum RoleLinkType
    {
        And,
        Or
    }

}
