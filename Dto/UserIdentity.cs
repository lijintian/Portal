using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Dto
{
    /// <summary>
    /// 表示用户身份标识
    /// </summary>
    public class UserIdentity
    {
        public static readonly UserIdentity AnonymousIdentity = new UserIdentity(string.Empty, string.Empty, UserType.Customer, string.Empty);
        public UserIdentity(string loginName, string displayName, UserType type, string token)
        {
            this.LoginName = loginName;
            this.DisplayName = displayName;
            this.Token = token;
            this.UserType = type;
        }

        /// <summary>
        /// 登录名
        /// </summary>
        public string LoginName { get; private set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string DisplayName { get; private set; }

        /// <summary>
        /// 用户类型
        /// </summary>
        public UserType UserType { get; private set; }

        /// <summary>
        /// 登录token
        /// </summary>
        public string Token { get; private set; }

        /// <summary>
        /// 查询登录是否成功
        /// </summary>
        /// <returns>查询结果</returns>
        public bool IsLoginSuccessed()
        {
            return !string.IsNullOrWhiteSpace(this.LoginName);
        }
    }
}
