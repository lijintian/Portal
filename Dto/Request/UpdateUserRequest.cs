using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Dto.Request
{
    /// <summary>
    /// 表示更改用户信息请求
    /// </summary>
    public class UpdateUserRequest
    {
        /// <summary>
        /// 显示名称
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// 邮箱地址
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 客户代码
        /// </summary>
        public string ClientNo { get; set; }

        /// <summary>
        /// 描述信息
        /// </summary>
        public string Desc { get; set; }
    }
}
