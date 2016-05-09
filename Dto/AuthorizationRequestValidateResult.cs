using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Portal.Dto
{
    /// <summary>
    /// 表示Oauth授权请求验证结果
    /// </summary>
    public class AuthorizationRequestValidateResult
    {
        public AuthorizationRequestValidateResult()
        {
            this.IsOk = true;
            this.Message = string.Empty;
        }

        /// <summary>
        /// 验证结果
        /// </summary>
        public bool IsOk
        {
            get;
            set;
        }

        /// <summary>
        /// 验证消息
        /// </summary>
        public string Message
        {
            get;
            set;
        }
    }
}
