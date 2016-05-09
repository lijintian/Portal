using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Dto
{
    /// <summary>
    /// 表示角色信息查找请求对象
    /// </summary>
    public class FindRoleRequest : PagerFindRequest
    {
        public string ApplicationId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
    }
}
