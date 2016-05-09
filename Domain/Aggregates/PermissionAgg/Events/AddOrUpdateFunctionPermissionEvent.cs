

using EasyDDD.Core.Event;

namespace Portal.Domain.Aggregates.PermissionAgg.Events
{
    public class AddOrUpdateFunctionPermissionEvent : DomainEvent
    {
        public FunctionPermissionInfo[] FunctionPermissions { get; private set; }

        public AddOrUpdateFunctionPermissionEvent(PagePermission pagePermission, FunctionPermissionInfo[] functionPermissionInfos)
            : base(pagePermission)
        {
            this.FunctionPermissions = functionPermissionInfos;
        }
    }
}
