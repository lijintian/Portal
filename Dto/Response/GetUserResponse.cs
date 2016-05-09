using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using Portal.Dto.Common;

namespace Portal.Dto.Response
{
    /// <summary>
    /// 表示用户信息查询响应
    /// </summary>
    public class GetUserResponse : ResponseBase
    {
        public static readonly GetUserResponse Empty = new GetUserResponse();
        public GetUserResponse()
        {
            this.RoleCodes = new List<string>();
            this.PermissionCodes = new List<string>();
        }
        /// <summary>
        /// 登录名称
        /// </summary>
        public string LoginName { get; set; }

        /// <summary>
        /// 内部员工工号
        /// </summary>
        public string EmployeeNo { get; set; }

        /// <summary>
        /// 客户代码
        /// </summary>
        public string ClientNo { get; set; }

        /// <summary>
        /// 显示名称
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// 邮箱地址
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 描述信息
        /// </summary>
        public string Desc { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsApproved { get; set; }

        /// <summary>
        /// 用户类型
        /// </summary>
        public string UserType { get; set; }
       
        /// <summary>
        /// 用户角色码列表
        /// </summary>
        public IEnumerable<string> RoleCodes { get; set; }

        /// <summary>
        /// 用户权限码列表
        /// </summary>
        public IEnumerable<string> PermissionCodes { get; set; }

        /// <summary>
        /// 是否有效的响应
        /// </summary>
        public bool IsValid()
        {
            return !string.IsNullOrEmpty(this.LoginName);
        }
    }
}
