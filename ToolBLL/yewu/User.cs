using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolBLL.yewu
{
    public class User
    {
        public long UserID { get; set; }
        /// <summary>
        /// 登录名称
        /// </summary>
        public string LoginName { get; set; }

        /// <summary>
        /// 兼容老的业务系统（员工的工号）
        /// </summary>
        public string EmployeeNo { get; set; }

        /// <summary>
        /// 客户代码
        /// </summary>
        public string ClientNo { get; set; }

        /// <summary>
        /// 邮件地址
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 显示名称
        /// </summary>

        public string DisplayName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Desc { get; set; }

        /// <summary>
        /// 登录IP
        /// </summary>
        public string LastLoginedIp { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedOn { get; set; }

        /// <summary>
        /// 最后登录时间
        /// </summary>
        public DateTime? LastLoginedTime { get; set; }

        /// <summary>
        /// 是否可用
        /// </summary>
        public bool IsApproved { get; set; }
        /// <summary>
        /// 使用 “,”号分割
        /// </summary>
        public string Purview { get; set; }
        public int ClientID { get; set; }
    }
}
