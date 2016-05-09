using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolBLL.portal
{
    public class User
    {
        public User()
        {

            this._id = ObjectId.GenerateNewId().ToString();
            this.FailedPasswordAttemptCount = 0;
            this.Password = string.Empty;
            this.IsApproved = false;
            this.IsLocked = false;
            this.Desc = string.Empty;
        }

        public string _id { get; set; }
        /// <summary>
        /// 登录名称
        /// </summary>
        public string LoginName { get; set; }

        /// <summary>
        /// 兼容老的业务系统（员工的工号）
        /// </summary>
        public string EmployeeNo { get; set; }

        /// <summary>
        /// 客户代码
        /// </summary>
        public string ClientNo { get; set; }

        /// <summary>
        /// 邮件地址
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 显示名称
        /// </summary>

        public string DisplayName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Desc { get; set; }

        /// <summary>
        /// 登录IP
        /// </summary>
        public string LastLoginedIp { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedOn { get; set; }

        /// <summary>
        /// 最后登录时间
        /// </summary>
        public DateTime? LastLoginedTime { get; set; }

        /// <summary>
        /// 是否可用
        /// </summary>
        public bool IsApproved { get; set; }

        /// <summary>
        /// 是否锁定
        /// </summary>
        public bool IsLocked { get; set; }

        /// <summary>
        /// 尝试重试密码失败次数
        /// </summary>
        public int FailedPasswordAttemptCount { get; set; }

        /// <summary>
        /// 用户类型
        /// </summary>
        public UserType UserType { get; set; }

        public List<string> Permissions { get; set; }
        public List<string> Roles { get; set; }
        public string _t { get; set; }
        public byte[] RowVersion { get; set; }
    }

    public enum UserType
    {
        Customer,
        Employee,
        Api
    }
}
