using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Portal.Dto;

namespace Portal.SDK.Security
{
    /// <summary>
    /// 表示用户整合信息，包含用户拥有的所有角色和权限
    /// </summary>
    public class UserPackageInfo
    {
        public UserPackageInfo(string loginName, string displayName, string token, UserType type,
            IEnumerable<string> roles, IEnumerable<string> permissions)
        {
            this.LoginName = loginName;
            this.DisplayName = displayName;
            this.Token = token;
            this.UserType = type;
            this.RoleCodes = (roles ?? Enumerable.Empty<string>()).ToList().AsReadOnly();
            this.PermissionCodes = (permissions ?? Enumerable.Empty<string>()).ToList().AsReadOnly();
        }

        /// <summary>
        /// 登录名称
        /// </summary>
        public string LoginName { get; private set; }

        /// <summary>
        /// 显示名称
        /// </summary>
        public string DisplayName { get; private set; }

        /// <summary>
        /// 表示用户token
        /// </summary>
        public string Token { get; private set; }

        /// <summary>
        /// 用户类型
        /// </summary>
        public UserType UserType { get; private set; }

        /// <summary>
        /// 用户角色码列表
        /// </summary>
        public ReadOnlyCollection<string> RoleCodes { get; private set; }

        /// <summary>
        /// 用户权限码列表
        /// </summary>
        public ReadOnlyCollection<string> PermissionCodes { get; private set; }
    }
}
