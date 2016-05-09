using EasyDDD.Infrastructure.Crosscutting.Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Applications.Events
{
    /// <summary>
    /// 表示应用层事件
    /// </summary>
    public class ApplicationEvent : IEvent
    {
        public Guid ID
        {
            get { return Guid.NewGuid(); }
        }

        public DateTime TimeStamp
        {
            get { return DateTime.Now; }
        }
    }
}
