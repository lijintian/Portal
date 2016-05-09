using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolBLL.portal
{
    public class PortalInfo
    {
        /// <summary>
        /// 
        /// </summary>
        public PortalInfoType Type { get; set; }

        /// <summary>
        /// PermissionId 只有在创建新权限时有效
        /// </summary>
        public string ModifiedId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime UpdateOn { get; set; }

    }
    public enum PortalInfoType
    {
        UserInfo = 1,//          表示用户信息变更
        UserPermission = 2,//    表示用户权限变更
        Permission = 3//         表示创建新权限  
    }
}
