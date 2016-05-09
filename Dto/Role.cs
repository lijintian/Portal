using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Dto
{
    /// <summary>
    /// 表示角色传输对象
    /// </summary>
    public class Role : DomainDto
    {
        #region 属性
        public string Name { get; set; }
        public string Code { get; set; }
        public string Desc { get; set; }
        public string ApplicationId { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public string CreatedByUserId { get; set; }

        public IEnumerable<string> PermissionCode { get; set; }
        #endregion

        #region 方法
        public void Add(string[] codes)
        {
            PermissionCode = HasPermissionCode() ? PermissionCode.Union(codes) : codes;
        }

        public bool HasPermissionCode()
        {
            return PermissionCode != null && PermissionCode.Any();
        }
        #endregion
    }
}
