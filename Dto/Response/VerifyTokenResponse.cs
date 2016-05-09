using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Portal.Dto.Common;

namespace Portal.Dto.Response
{
    /// <summary>
    /// 检查token响应
    /// </summary>
    public class VerifyTokenResponse : ResponseBase
    {
        /// <summary>
        /// 登录名称
        /// </summary>
        public string LoginName { get; set; }

        /// <summary>
        /// 是否成功
        /// </summary>
        public bool IsSuccessed { get; set; }

        /// <summary>
        /// 显示名称
        /// </summary>
        public string DisplayName { get; set; }
    }
}
