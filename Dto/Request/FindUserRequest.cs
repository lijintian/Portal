using System;
using System.ComponentModel;

namespace Portal.Dto
{
    /// <summary>
    /// 表示用户查找请求
    /// </summary>
    public class FindUserRequest : PagerFindRequest
    {
        #region 属性
        /// <summary>
        /// 用户登录名称
        /// </summary>
        public string LoginName { get; set; }
        /// <summary>
        /// 用户类型
        /// </summary>
        public UserType UserType { get; set; }

        /// <summary>
        /// 是否为内外部API账号
        /// </summary>
        public bool IsApi { get; set; }

        /// <summary>
        /// 兼容老的业务系统（员工的工号）
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
        /// 员工号列表
        /// </summary>
        public string[] Employees { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 联系电话
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// 权限码
        /// </summary>
        public string PermissionCode { get; set; }

        /// <summary>
        /// 账号创建时间查询起始时间
        /// </summary>
        public DateTime? FromCreatedDateTime { get; set; }
        /// <summary>
        /// 账号创建时间查询结束时间
        /// </summary>
        public DateTime? ToCreatedDateTime { get; set; }
        #endregion

        #region 初始化
        public FindUserRequest()
            : base()
        {
            this.UserType = UserType.Customer;
        }
        #endregion
    }

    public enum UserSearchType
    {
        [Description("登陆名称")]
        LoginName = 1,

        [Description("员工号")]
        EmployeeNo = 2,

        [Description("员工名称")]
        DisplayName = 3,

        [Description("邮箱")]
        Email = 4,

        [Description("权限码")]
        PermissionCode = 5,
    }
}
