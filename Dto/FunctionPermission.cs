using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Dto
{
    /// <summary>
    /// 表示功能权限
    /// </summary>
    public class FunctionPermission : Permission
    {
        public string ParentId { get; set; }
    }
}
