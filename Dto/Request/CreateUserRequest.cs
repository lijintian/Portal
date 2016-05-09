using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Dto.Request
{
    /// <summary>
    /// 表示远程请求创建一个账号请求参数
    /// </summary>
    public class CreateUserRequest
    {
        /// <summary>
        /// 登录名称
        /// </summary>
        public string LoginName { get; set; }

        /// <summary>
        /// 内部员工工号
        /// </summary>
        public string EmployeeNo { get; set; }

        /// <summary>
        /// 客户代码
        /// </summary>
        public string ClientNo { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }
        
        /// <summary>
        /// 显示名称
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// 邮箱地址
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 描述信息
        /// </summary>
        public string Desc { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsApproved { get; set; }

        /// <summary>
        /// 是否客户用户
        /// </summary>
        public bool IsCustomer { get; set; }
    }
}
