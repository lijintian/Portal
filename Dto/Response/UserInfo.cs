using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Dto.Response
{
    /// <summary>
    /// 表示返回的用户信息
    /// </summary>
    public class UserInfo
    {
        /// <summary>
        /// 登录名称
        /// </summary>
        public string LoginName
        {
            get;
            set;
        }

        /// <summary>
        /// 用户名称
        /// </summary>
        public string DispalyName
        {
            get;
            set;
        }

        /// <summary>
        /// 员工编号
        /// </summary>
        public string EmployeeNo
        {
            get;
            set;
        }
    }
}
