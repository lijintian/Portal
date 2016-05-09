using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Portal.Dto.Response
{
    /// <summary>
    ///  表示Oauth授权响应
    /// </summary>
    public class AuthorizationResponse
    {
        /// <summary>
        /// 表示响应的类型
        /// </summary>
        public bool IsUseAuthorizationCode
        {
            get;
            set;
        }

        /// <summary>
        /// 表示生成的授权码或AccessToken
        /// </summary>
        public string CodeOrToken
        {
            get;
            set;
        }       
    }
}
