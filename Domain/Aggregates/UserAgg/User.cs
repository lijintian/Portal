



using EasyDDD.Core.Aggregate;
using EasyDDD.Core.Event;
using EasyDDD.Infrastructure.Crosscutting.Helpers;
using EasyDDD.Infrastructure.Crosscutting.InversionOfControl;
using Portal.Domain.Aggregates.PermissionAgg.Events;
using Portal.Domain.Aggregates.PermissionAgg.Events.Callbacks;
using Portal.Domain.Aggregates.RoleAgg.Events;
using Portal.Domain.Aggregates.RoleAgg.Events.Callbacks;
using Portal.Domain.Aggregates.UserAgg.Events;
using Portal.Domain.Aggregates.UserAgg.Events.Callbacks;
using Portal.Domain.Aggregates.UserAgg.State;
using Portal.Domain.Aggregates.UserAgg.Strategies;
using Portal.Infrastructure.Exceptions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Portal.Domain.Aggregates.UserAgg
{
    /// <summary>
    /// Represents a user of the system
    /// </summary>
    public class User : AggregateRoot
    {
        public User(string loginName)
        {
            this.GenerateNewIdentity();
            this.FailedPasswordAttemptCount = 0;
            this.Password = string.Empty;
            this.IsApproved = false;
            this.IsLocked = false;
            this.Desc = string.Empty;
            this.PhoneNumber = string.Empty;
            this._roles = new List<string>();
            this._permissions = new List<string>();
            SetLoginName(loginName);
        }

        public User(string loginName, UserType userType)
            : this(loginName)
        {
            this.UserType = userType;
            SetDefaultRole();
            SetDefaultPermission();
        }


        public User(string loginName, string password, UserType userType)
            : this(loginName)
        {
            ChangePassword(password);
            this.UserType = userType;
            SetDefaultRole();
            SetDefaultPermission();
        }

        /// <summary>
        /// 登录名称
        /// </summary>
        public string LoginName { get; private set; }

        /// <summary>
        /// 兼容老的业务系统（员工的工号）
        /// </summary>
        public string EmployeeNo { get; private set; }

        /// <summary>
        /// 客户代码
        /// </summary>
        public string ClientNo { get; private set; }

        /// <summary>
        /// 邮件地址
        /// </summary>
        public string Email { get; private set; }

        /// <summary>
        /// 联系电话
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// 显示名称
        /// </summary>

        public string DisplayName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; private set; }

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
        public bool IsApproved { get; private set; }

        /// <summary>
        /// 是否锁定
        /// </summary>
        public bool IsLocked { get; private set; }

        /// <summary>
        /// 尝试重试密码失败次数
        /// </summary>
        public int FailedPasswordAttemptCount { get; private set; }

        public string CreatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public string UpdatedBy { get; set; }

        /// <summary>
        /// 用户类型
        /// </summary>
        public UserType UserType { get; set; }


        /// <summary>
        /// 状态
        /// </summary>
        public UserState State
        {
            get
            {
                UserState state;

                if (IsApproved)
                {
                    state = new EnabledState(this);
                }
                else
                {
                    state = new DisabledState(this);
                }

                return state;
            }
        }


        #region 方法
        /// <summary>
        /// 设置默认角色
        /// </summary>
        private void SetDefaultRole()
        {
            string defaultRoles = string.Empty;
            switch (this.UserType)
            {
                case UserType.Customer:
                    defaultRoles = System.Configuration.ConfigurationManager.AppSettings["CustumerDefaultRole"];
                    break;
                case UserType.Employee:
                    defaultRoles = System.Configuration.ConfigurationManager.AppSettings["EmployeeDefaultRole"];
                    break;
                case UserType.InternalApi:
                    defaultRoles = System.Configuration.ConfigurationManager.AppSettings["ApiUserDefaultRole"];
                    break;
                case UserType.ExternalApi:
                    defaultRoles = System.Configuration.ConfigurationManager.AppSettings["ExternalApiUserDefaultRole"];
                    break;
            }
            if (!string.IsNullOrEmpty(defaultRoles))
            {
                AddRoleRange(defaultRoles.Split(','));
            }
        }

        /// <summary>
        /// 设置默认权限
        /// </summary>
        private void SetDefaultPermission()
        {
            string defaultPermissions = string.Empty;
            switch (this.UserType)
            {
                case UserType.Customer:
                    defaultPermissions = System.Configuration.ConfigurationManager.AppSettings["CustumerDefaultPermission"];
                    break;
                case UserType.Employee:
                    defaultPermissions = System.Configuration.ConfigurationManager.AppSettings["EmployeeDefaultPermission"];
                    break;
                case UserType.InternalApi:
                    defaultPermissions = System.Configuration.ConfigurationManager.AppSettings["ApiUserDefaultPermission"];
                    break;
                case UserType.ExternalApi:
                    defaultPermissions = System.Configuration.ConfigurationManager.AppSettings["ExternalApiUserDefaultPermission"];
                    break;
            }
            if (!string.IsNullOrEmpty(defaultPermissions))
            {
                AddPermisssionRange(defaultPermissions.Split(','));
            }
        }

        public void ChangePassword(string oldPassword, string newPassword)
        {
            Check.Argument.IsNotNull(oldPassword, "oldPassword");
            Check.Argument.IsNotNull(newPassword, "newPassword");
            if (IsMatchedPasword(oldPassword))
            {
                ChangePassword(newPassword);
            }
            else
            {
                throw new PortalException(ErrorCodes.StringCodes.OldPassowrdNotMatch, ErrorMessage.OldPassowrdNotMatch);
            }
        }
        /// <summary>
        /// 改变Password
        /// </summary>
        /// <param name="newPassword">新密码</param>
        public void ChangePassword(string newPassword)
        {
            if (string.IsNullOrEmpty(newPassword))
            {
                throw new PortalException(ErrorCodes.StringCodes.PasswordCannotBeNull, ErrorMessage.PasswordCannotBeNull);
            }
            //加密
            this.Password = EncryptPassword(newPassword);
        }
        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="checkEmail">检查Email格式是否正确</param>
        public void ResetPassword(bool checkEmail = false)
        {
            var newPassword = GenRandomPassword();
            this.Password = EncryptPassword(newPassword);
            string emailTo = this.Email;
            if (checkEmail && (string.IsNullOrEmpty(emailTo) || !RegexHelper.IsEmail(emailTo)))//!CheckEmail(this.Email)
            {
                emailTo = System.Configuration.ConfigurationManager.AppSettings["DefaultSendEmail"];
                if (string.IsNullOrEmpty(emailTo))
                {
                    return;
                }
            }
            DomainEvent.Publish<ResetedPasswordEvent>(new ResetedPasswordEvent(this, newPassword, emailTo));
        }


        public void SetEmployeeNo(string employeeNo)
        {
            if (this.EmployeeNo == employeeNo) return;

            if (string.IsNullOrEmpty(employeeNo))
            {
                this.EmployeeNo = "";
            }
            else
            {
                DomainEvent.Publish<ValidateUserExistsSameEmployeeNoEvent, ValidateUserExistsSameEmployeeNoEventResult>
                   (
                       new ValidateUserExistsSameEmployeeNoEvent(this, employeeNo),
                       (e) =>
                       {
                           if (e != null)
                           {
                               if (e.Exists)
                               {
                                   throw new PortalException(ErrorCodes.StringCodes.ExistsSameEmployeeNo, ErrorMessage.ExistsSameEmployeeNo.FormatWith(employeeNo));
                               }
                               this.EmployeeNo = employeeNo;
                           }
                       }
                   );
            }
        }

        public void SetClientNo(string clientNo)
        {
            this.ClientNo = clientNo;
        }

        /// <summary>
        /// 设置邮箱地址
        /// </summary>
        /// <param name="email"></param>
        public void SetEmail(string email)
        {
            if (this.Email != email && CheckEmail(email))
            {
                this.Email = email;
            }
        }

        /// <summary>
        /// 设置手机号码
        /// </summary>
        /// <param name="phone"></param>
        public void SetPhone(string phone)
        {
            CheckArgument.IsNotNullOrEmpty(phone, "手机号码");
            if (this.Phone != phone && CheckPhone(phone))
            {
                this.Phone = phone;
            }
        }

        /// <summary>
        /// 判断密码是否正确
        /// </summary>
        /// <param name="password">输入的密码</param>
        /// <returns></returns>
        public bool IsMatchedPasword(string password)
        {
            var strategy = IoC.Resolve<IPasswordEncryptStrategy>();
            Check.Argument.IsNotNull(strategy, "PasswordEncryptStrategy");

            return this.Password == strategy.Encrypt(password);
        }

        public bool IsMatchedHashPassword(string hashPassword)
        {
            return this.Password == hashPassword;
        }

        internal void SetState(UserState state)
        {
            if (state is EnabledState)
            {
                IsApproved = true;
            }
            else
            {
                IsApproved = false;
            }
        }

        /// <summary>
        /// 启动
        /// </summary>
        public void Enable()
        {
            this.State.Enable();
        }

        /// <summary>
        /// 禁用
        /// </summary>

        public void Disable()
        {
            this.State.Disable();
        }

        public bool IsOverMaxTryLoginFailCount()
        {
            return this.FailedPasswordAttemptCount > 5;
        }

        private List<string> _roles;

        public ReadOnlyCollection<string> Roles
        {
            get
            {
                if (_roles == null) _roles = new List<string>();
                return _roles.AsReadOnly();
            }
            private set
            {
                if (value == null)
                {
                    _roles = new List<string>();
                }
                else
                {
                    _roles = value.ToList();
                }
            }
        }

        private List<string> _permissions;

        public ReadOnlyCollection<string> Permissions
        {
            get
            {
                if (_permissions == null) _permissions = new List<string>();
                return _permissions.AsReadOnly();
            }
            private set
            {
                if (value == null)
                {
                    _permissions = new List<string>();
                }
                else
                {
                    _permissions = value.ToList();
                }
            }
        }

        /// <summary>
        /// 添加角色
        /// </summary>
        /// <param name="roleCode"></param>
        public void AddRole(string roleCode)
        {
            if (_roles.Contains(roleCode)) return;

            ValidateRoleExists(new[] { roleCode });
            _roles.Add(roleCode);
        }

        /// <summary>
        /// 添加角色
        /// </summary>
        /// <param name="roleCodes"></param>
        public void AddRoleRange(string[] roleCodes)
        {
            if (roleCodes.Length == 0) return;
            string[] newRoleCodes = roleCodes.Except(_roles).ToArray();


            ValidateRoleExists(newRoleCodes);
            _roles.AddRange(roleCodes);
        }

        /// <summary>
        /// 清空角色
        /// </summary>
        public void ClearRoles()
        {
            _roles.Clear();
        }

        /// <summary>
        /// 添加用户权限
        /// </summary>
        /// <param name="permissionCode"></param>
        public void AddPermisssion(string permissionCode)
        {
            if (_permissions.Contains(permissionCode)) return;

            ValidatePermissionExists(new[] { permissionCode });
            _permissions.Add(permissionCode);
        }

        /// <summary>
        /// 添加用户权限
        /// </summary>
        /// <param name="permissionCodes"></param>
        public void AddPermisssionRange(string[] permissionCodes)
        {
            if (permissionCodes.Length == 0) return;
            string[] newPersCodes = permissionCodes.Except(_permissions).ToArray();


            ValidatePermissionExists(newPersCodes);
            _permissions.AddRange(permissionCodes);
        }

        /// <summary>
        /// 清空用户权限
        /// </summary>
        public void ClearPermisssions()
        {
            _permissions.Clear();
        }


        #endregion

        #region 私有方法

        private void SetLoginName(string loginName)
        {
            CheckArgument.IsNotNullOrEmpty(loginName, "登录名称");
            if (this.LoginName == loginName) return;

            DomainEvent.Publish<ValidateUserExistsSameLoginNameEvent, ValidateUserExistsSameLoginNameEventResult>
            (
                new ValidateUserExistsSameLoginNameEvent(this, loginName),
                (e) =>
                {
                    if (e != null)
                    {
                        if (e.Exists)
                        {
                            throw new PortalException(ErrorCodes.StringCodes.ExistsSameUserLoginName, ErrorMessage.ExistsSameUserLoginName.FormatWith(loginName));
                        }
                        this.LoginName = loginName;
                    }
                }
            );
        }

        /// <summary>
        /// 加密Password
        /// </summary>
        /// <param name="password"></param>
        private string EncryptPassword(string password)
        {
            var strategy = IoC.Resolve<IPasswordEncryptStrategy>();
            Check.Argument.IsNotNull(strategy, "PasswordEncryptStrategy");

            return strategy.Encrypt(password);
        }

        /// <summary>
        /// 解除锁定
        /// </summary>
        private void UnLock()
        {
            if (IsLocked) IsLocked = false;
            this.FailedPasswordAttemptCount = 0;
        }

        private void ValidateRoleExists(string[] roleCodes)
        {
            DomainEvent.Publish<ValidateRoleExistsEvent, ValidateRoleExistsEventResult>(
                new ValidateRoleExistsEvent(roleCodes),
                e =>
                {
                    if (e != null)
                    {
                        if (e.NoFoundRoleCodes != null && e.NoFoundRoleCodes.Any())
                        {
                            string codes = string.Join(",", e.NoFoundRoleCodes);
                            throw new PortalException(ErrorCodes.StringCodes.NoFoundRoleCode, ErrorMessage.NoFoundRoleCode.FormatWith(codes));
                        }
                    }
                });
        }


        private void ValidatePermissionExists(string[] permissionCodes)
        {
            ValidatePermissionExistsEvent parameter = this.UserType == UserType.InternalApi ? new ValidatePermissionExistsEvent(permissionCodes, false, true) : new ValidatePermissionExistsEvent(permissionCodes);
            DomainEvent.Publish<ValidatePermissionExistsEvent, ValidatePermissionExistsEventResult>(parameter,
                e =>
                {
                    if (e != null)
                    {
                        if (!e.Success)
                        {
                            throw new PortalException(ErrorCodes.StringCodes.NoFoundPermissionCode, e.ErrorMessage);
                        }
                    }
                });
        }

        private string GenRandomPassword()
        {

            System.Threading.Thread.Sleep(3);

            char[] pattern = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
            string result = "";
            int n = pattern.Length;
            System.Random random = new Random(~unchecked((int)DateTime.Now.Ticks));
            for (int i = 0; i < 6; i++)
            {
                int rnd = random.Next(0, n);
                result += pattern[rnd];
            }
            return result;
        }

        /// <summary>
        /// 检查Email格式是否正确
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        private bool CheckEmail(string email)
        {
            CheckArgument.IsNotNullOrEmpty(email, "邮箱");
            if (RegexHelper.IsEmail(email))
            {
                return true;
            }
            else
            {
                throw new PortalException(ErrorCodes.StringCodes.InvalidEmail, ErrorMessage.InvalidEmail);
            }
            return false;
        }

        /// <summary>
        /// 检查手机号码格式是否正确
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        private bool CheckPhone(string phone)
        {
            CheckArgument.IsNotNullOrEmpty(phone, "手机号码");
            if (RegexHelper.IsMatch(@"^(13[0-9]|15[0-9]|18[0-9])\d{8}$", phone))
            {
                return true;
            }
            else
            {
                throw new PortalException(ErrorCodes.StringCodes.InvalidPhone, ErrorMessage.InvalidPhone);
            }
            return false;
        }
        #endregion
    }
}
