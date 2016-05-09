using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Dto
{
    /// <summary>
    /// 表示查询用户响应
    /// </summary>
    public class FindUserResponse : PagerFindResponse<User>
    {
        public FindUserResponse()
        {
            
        }
        public FindUserResponse(int total, IEnumerable<User> users)
            : base(total, users)
        {
        }
    }
}
