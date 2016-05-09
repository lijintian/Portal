using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Dto
{
    /// <summary>
    /// 表示用户登录信息传输对象
    /// </summary>
    public class LoginInfo
    {
        public LoginInfo()
        {
            this.IsHashPassword = false;
        }
        /// <summary>
        /// 登录用户名或employeeno
        /// </summary>
        public string Identity { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// IP地址
        /// </summary>
        public string Ip { get; set; }

        /// <summary>
        /// 访问来源
        /// </summary>
        public string Source { get; set; }

        public bool IsHashPassword { get; set; }

        /// <summary>
        /// 登录时间
        /// </summary>
        public DateTime LoginDateTime
        {
            get
            {
                return DateTime.Now;
            }
        }

    }
}
