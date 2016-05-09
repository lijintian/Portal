using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Portal.Dto.Common;

namespace Portal.Dto.Response
{
    /// <summary>
    /// 表示获取用户列表响应
    /// </summary>
    public class GetUsersResponse : ResponseBase
    {
        public GetUsersResponse()
        {
            this.Users = new UserInfo[]{};
        }

        public UserInfo[] Users
        {
            get;
            set;
        }
    }
}
