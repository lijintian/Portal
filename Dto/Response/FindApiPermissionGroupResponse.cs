using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Portal.Dto
{
    public class FindApiPermissionGroupResponse : PagerFindResponse<ApiPermissionGroup>
    {
        public FindApiPermissionGroupResponse(int total, IEnumerable<ApiPermissionGroup> groups)
            : base(total, groups)
        {
        }
    }
}
