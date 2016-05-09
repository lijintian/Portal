

using EasyDDD.Core.Event;

namespace Portal.Domain.Aggregates.PermissionAgg.Events.Callbacks
{
    public class ValidatePermissionExistsEventResult : IDomainEventResult
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool Success { get; private set; }

        /// <summary>
        /// 错误消息
        /// </summary>
        public string ErrorMessage { get; private set; }

        /// <summary>
        /// 有效的权限码
        /// </summary>
        public string[] PermissionCodes { get; set; }

        /// <summary>
        /// 父节点权限ID
        /// </summary>
        public string[] ParentPermissionCodes { get; set; }

        #region 初始化
        public ValidatePermissionExistsEventResult(string errorMessage)
        {
            this.Success = string.IsNullOrEmpty(errorMessage);
            this.ErrorMessage = errorMessage;
        }
        #endregion

        #region 方法
        public void Init(string errorMessage)
        {
            this.Success = false;
            this.ErrorMessage = string.Format(string.IsNullOrEmpty(this.ErrorMessage) ? "{1}" : "{0}；{1}", this.ErrorMessage, errorMessage);
        }
        #endregion
    }
}
