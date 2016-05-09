using System;
using System.ComponentModel;

namespace Portal.Dto
{
    /// <summary>
    /// 表示用户信息
    /// </summary>
    public class User : DomainDto
    {
        public User()
        {
            this.CreatedOn = DateTime.Now;
            this.IsApproved = false;
        }
        /// <summary>
        /// 登录名称
        /// </summary>
        public string LoginName { get; set; }

        /// <summary>
        /// 兼容老系统，内部员工工号
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
        /// 邮件地址
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 联系电话
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 原始密码
        /// </summary>
        public string OldPassword { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsApproved { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Desc { get; set; }

        /// <summary>
        /// ip地址
        /// </summary>
        public string LoginIp { get; set; }

        /// <summary>
        /// 用户类型
        /// </summary>
        public UserType UserType { get; set; }

    }

    /// <summary>
    /// 用户类型
    /// </summary>
    public enum UserType
    {
        [Description("客户帐号")]
        Customer,

        [Description("内部帐号")]
        Employee,

        [Description("内部API帐号")]
        InternalApi,

        [Description("外部API帐号")]
        ExternalApi,
    }
}
