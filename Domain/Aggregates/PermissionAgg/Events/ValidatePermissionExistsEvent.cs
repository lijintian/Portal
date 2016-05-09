


using EasyDDD.Core.Event;
using EasyDDD.Infrastructure.Crosscutting.Helpers;

namespace Portal.Domain.Aggregates.PermissionAgg.Events
{
    public class ValidatePermissionExistsEvent : DomainEvent
    {
        #region 属性
        public string[] PermissionCodes { get; private set; }

        /// <summary>
        /// 开放的权限
        /// </summary>
        public bool IsOpend { get; private set; }

        /// <summary>
        /// 不需要客户授权的权限
        /// </summary>
        public bool IsUnCustomerGranted { get; private set; }

        /// <summary>
        /// 所属应用程序
        /// </summary>
        public string ApplicationId { get; private set; }

        /// <summary>
        /// 是否获取父节点权限代码
        /// </summary>
        public bool IsGetParentPermissionCodes { get; private set; }
        #endregion

        #region 初始化
        public ValidatePermissionExistsEvent(string[] permissionCodes)
        {
            Check.Argument.IsNotNull(permissionCodes, "permissionCodes");
            this.PermissionCodes = permissionCodes;
        }

        public ValidatePermissionExistsEvent(string[] permissionCodes, bool isOpened, bool isUnCustomerGranted)
            : this(permissionCodes)
        {
            this.IsOpend = isOpened;
            this.IsUnCustomerGranted = isUnCustomerGranted;
        }

        public ValidatePermissionExistsEvent(string[] permissionCodes, bool isGetParentCodes)
            : this(permissionCodes)
        {
            this.IsGetParentPermissionCodes = isGetParentCodes;
        }

        public ValidatePermissionExistsEvent(string[] permissionCodes, string applicationId, bool isGetParentCodes)
            : this(permissionCodes)
        {
            this.ApplicationId = applicationId;
            this.IsGetParentPermissionCodes = isGetParentCodes;
        }
        #endregion
    }
}
