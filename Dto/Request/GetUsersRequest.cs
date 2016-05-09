using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Portal.Dto.Request
{
    /// <summary>
    /// 表示获取用户请求
    /// </summary>
    public class GetUsersRequest
    {
        public GetUsersRequest()
        {
            this.Employees = new string[]{ };
            this.Name = string.Empty;
        }

        /// <summary>
        /// 员工列表
        /// </summary>
        public string[] Employees
        {
            get;
            set;
        }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get;
            set;
        }
    }
}
