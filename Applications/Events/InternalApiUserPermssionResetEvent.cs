using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Portal.Domain.Aggregates.UserAgg;

namespace Portal.Applications.Events
{
    /// <summary>
    /// 内部API账号权限变更事件
    /// </summary>
    public class InternalApiUserPermssionResetEvent : ApplicationEvent
    {
        public User User
        {
            get;
            private set;
        }

        public InternalApiUserPermssionResetEvent(User user)
        {
            this.User = user;
        }
    }
}
