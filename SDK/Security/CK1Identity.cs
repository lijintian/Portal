using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Security;

namespace Portal.SDK.Security
{
    /// <summary>
    /// 表示用户身份标识
    /// </summary>
    public class CK1Identity : IIdentity
    {
        private readonly string _name;
        public CK1Identity(string loginName)
        {
            this._name = loginName;
        }

        /// <summary>
        /// 验证的类型
        /// </summary>
        public string AuthenticationType
        {
            get { return "ck1Portal"; }
        }

        /// <summary>
        /// 是否通过验证
        /// </summary>
        public bool IsAuthenticated
        {
            get { return !string.IsNullOrEmpty(this.Name); }
        }

        /// <summary>
        /// 用户名
        /// </summary>
        public string Name
        {
            get { return this._name; }
        }
    }
}
