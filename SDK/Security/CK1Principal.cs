using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Security;
using Portal.Dto;

namespace Portal.SDK.Security
{
    /// <summary>
    /// 表示用户身份主体
    /// </summary>
    public class CK1Principal : IPrincipal, IPermissionCheck, IRoleCheck
    {
        private readonly CK1Identity _ck1Identity;
        protected UserPackageInfo UserInfo { get; private set; }

        /// <summary>
        /// 实例化用户主体对象
        /// </summary>
        /// <param name="userInfo"></param>
        public CK1Principal(UserPackageInfo userInfo)
        {
            this.UserInfo = userInfo;
            this._ck1Identity = new CK1Identity(userInfo.LoginName);
        }

        /// <summary>
        /// 身份标识
        /// </summary>
        public IIdentity Identity
        {
            get { return this._ck1Identity; }
        }

        /// <summary>
        /// 登录用户token
        /// </summary>
        public string Token
        {
            get
            {
                return this.UserInfo.Token;
            }
        }

        /// <summary>
        /// 显示名称
        /// </summary>
        public string DisplayName
        {
            get
            {
                return this.UserInfo.DisplayName;
            }
        }

        /// <summary>
        /// 用户类型
        /// </summary>
        public UserType UserType
        {
            get
            {
                return this.UserInfo.UserType;
            }
        }

        /// <summary>
        /// 是否外部客户
        /// </summary>
        public bool IsCustomer
        {
            get
            {
                return this.UserType == UserType.Customer;
            }
        }

        /// <summary>
        /// 用户是否拥有指定的角色
        /// </summary>
        public bool IsInRole(string role)
        {
            if (string.IsNullOrEmpty(role))
            {
                throw new ArgumentException("role");
            }
            return this.UserInfo.RoleCodes.Contains(role);
        }

        /// <summary>
        /// 用户是否拥有指定角色列表的所有角色
        /// </summary>
        public bool IsInAllRole(params string[] roles)
        {
            if (roles == null || roles.Length == 0)
            {
                throw new ArgumentException("roles");
            }
            return this.UserInfo.RoleCodes.Count(item => roles.Contains(item)) == roles.Length;
        }

        /// <summary>
        /// 用户是否拥有指定角色列表的任一角色
        /// </summary>
        public bool IsInAnyRole(params string[] roles)
        {
            if (roles == null || roles.Length == 0)
            {
                throw new ArgumentException("roles");
            }
            return this.UserInfo.RoleCodes.Count(item => roles.Contains(item)) > 0;
        }

        /// <summary>
        /// 用户是否拥有指定权限
        /// </summary>
        public bool HasPermission(string code)
        {
            if (string.IsNullOrEmpty(code))
            {
                throw new ArgumentException("code");
            }
            return this.UserInfo.PermissionCodes.Contains(code);
        }

        /// <summary>
        /// 用户是否拥有指定权限列表中的任一权限
        /// </summary>
        public bool HasAnyPermission(params string[] codes)
        {
            if (codes == null || codes.Length == 0)
            {
                throw new ArgumentException("codes");
            }
            return this.UserInfo.PermissionCodes.Count(item => codes.Contains(item)) > 0;
        }

        /// <summary>
        /// 用户是否拥有指定权限列表中所有权限
        /// </summary>
        public bool HasAllPermissions(params string[] codes)
        {
            if (codes == null || codes.Length == 0)
            {
                throw new ArgumentException("codes");
            }
            return this.UserInfo.PermissionCodes.Count(item => codes.Contains(item)) == codes.Length;
        }
    }
}
