
using EasyDDD.Core.Event;
using Portal.Domain.Model;

namespace Portal.Domain.Aggregates.ApiPermissionGroupAgg.Events.Callbacks
{
    public class ValidateApiPermissionGroupExistsEventResult : IDomainEventResult
    {
        /// <summary>
        /// API权限分组码
        /// </summary>
        public string[] GroupCode { get; set; }

        /// <summary>
        /// 系统不存在的角色Codes
        /// </summary>
        public string[] NoFoundApiPermissionGroupCodes { get; private set; }

        /// <summary>
        /// 验证是否成功
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// 权限码
        /// </summary>
        public ReferenceApiPermssionInfo[] PermissionCodes { get; set; }

        public ValidateApiPermissionGroupExistsEventResult(string[] codes)
        {
            this.Success = false;
            this.NoFoundApiPermissionGroupCodes = codes;
        }
        public ValidateApiPermissionGroupExistsEventResult(string[] groupCode, ReferenceApiPermssionInfo[] perermssions)
        {
            this.Success = true;
            this.GroupCode = groupCode;
            this.PermissionCodes = perermssions;
        }
    }
}
