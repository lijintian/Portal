

using EasyDDD.Core.Event;
using EasyDDD.Infrastructure.Crosscutting.Helpers;
using Portal.Domain.Aggregates.UserAgg;

namespace Portal.Domain.Aggregates.PermissionAgg.Events
{
    public class ValidatePermissionExistsSameNameEvent : DomainEvent
    {
        /// <summary>
        /// 应用系统ID
        /// </summary>
        public string ApplicationId { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 是否为API
        /// </summary>
        public bool IsApi { get; set; }

        public ValidatePermissionExistsSameNameEvent(Permission info,string applicationId, string name,bool isApi)
            : base(info)
        {
            Check.Argument.IsNotNull(applicationId, "applicationId");
            Check.Argument.IsNotNull(name, "name");
            Check.Argument.IsNotNull(isApi, "isApi");
            this.ApplicationId = applicationId;
            this.Name = name;
            this.IsApi = isApi;
        }
    }
}
