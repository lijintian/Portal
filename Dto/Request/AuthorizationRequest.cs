using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Portal.Dto.Request
{
    /// <summary>
    /// 表示Oauth授权请求
    /// </summary>
    public class AuthorizationRequest
    {
        /// <summary>
        /// 表示请求的响应类型 
        /// </summary>
        public string ResponseType
        {
            get;
            set;
        }

        /// <summary>
        /// 表示返回的重定向Url
        /// </summary>
        public string RedirectUri
        {
            get;
            set;
        }

        /// <summary>
        /// 表示开发者应用Id
        /// </summary>
        public string ClientId
        {
            get;
            set;
        }

        /// <summary>
        /// 表示授权的范围
        /// </summary>
        public string Scope
        {
            get;
            set;
        }

        /// <summary>
        /// 表示授权客户Id
        /// </summary>
        public string CustomerId
        {
            get;
            set;
        }
    }
}
