using System.Collections.Generic;
using System.Linq;

namespace Portal.Dto
{
    /// <summary>
    /// 
    /// </summary>
    public class FindPermissionResponse<T> : PagerFindResponse<T> where T : Permission
    {
        public FindPermissionResponse(int total, int totalPage, IEnumerable<T> list)
        {
            this.TotalRecords = total;
            this.TotalPages = totalPage;
            this.Data = list ?? Enumerable.Empty<T>();
        }
    }
}
