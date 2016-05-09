using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Portal.Dto.Common;

namespace Portal.Dto.Response
{
    /// <summary>
    /// 表示登录请求验证响应
    /// </summary>
    public class LoginInResponse : ResponseBase
    {
        public string LoginName { get; set; }
        public string DisplayName { get; set; }
        public bool IsCustomer { get; set; }
        public bool IsEmployee { get; set; }
        public bool IsApi { get; set; }
        public string Token { get; set; }

        /// <summary>
        /// 表示登录是否成功
        /// </summary>
        /// <returns></returns>
        public bool IsSuccessed()
        {
            return !string.IsNullOrWhiteSpace(this.Token);
        }
    }
}
