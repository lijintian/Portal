using System.Collections.Generic;
using System.Linq;

namespace Portal.Dto
{
    /// <summary>
    /// 表示角色查找响应对象
    /// </summary>
    public class FindRoleResponse : PagerFindResponse<Role>
    {
        public FindRoleResponse(int total, IEnumerable<Role> roles):base(total,roles)
        {
        }
    }
}
