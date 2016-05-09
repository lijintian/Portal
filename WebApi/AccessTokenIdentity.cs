using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Principal;

namespace Portal.WebApi
{
    /// <summary>
    /// 表示通过Access Token访问身份标识
    /// </summary>
    public class AccessTokenIdentity : IIdentity
    {
        public static readonly string AccessToken = "AccessToken";
        private readonly string _customerNo;
        private readonly string _customerName;
        private readonly string _appId;
        public AccessTokenIdentity(string customerNo, string customerName, string appId)
        {
            this._customerNo = customerNo;
            this._customerName = customerName;
            this._appId = appId;
        }

        /// <summary>
        /// 验证类型
        /// </summary>
        public string AuthenticationType
        {
            get { return AccessToken; }
        }

        /// <summary>
        /// 是否验证通过
        /// </summary>
        public bool IsAuthenticated
        {
            get { return true; }
        }
        
        /// <summary>
        /// 客户名称
        /// </summary>
        public string Name
        {
            get { return this._customerName; }
        }

        /// <summary>
        /// 应用Id
        /// </summary>
        public string AppClientId
        {
            get
            {
                return this._appId;
            }
        }
        
        /// <summary>
        /// 客户代码
        /// </summary>
        public string CustomerNo
        {
            get
            {
                return this._customerNo;
            }
        }
    }
}
