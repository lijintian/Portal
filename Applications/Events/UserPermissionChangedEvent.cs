using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Portal.Domain.Aggregates.SynchronizationAgg;

namespace Portal.Applications.Events
{
    public class UserPermissionChangedEvent : SynchronizationEvent
    {
        public UserPermissionChangedEvent(string id)
        {
            this.ModifiedId = id;
            this.Type = ModifiedType.UserPermission;
        }
    }
}
