using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Portal.Domain.Aggregates.SynchronizationAgg;

namespace Portal.Applications.Events
{
    public class UserInfoChangedEvent : SynchronizationEvent
    {
        public UserInfoChangedEvent(string id)
        {
            this.ModifiedId = id;
            this.Type = ModifiedType.UserInfo;
        }
    }
}
