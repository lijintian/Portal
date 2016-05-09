using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Portal.Domain.Aggregates.SynchronizationAgg;

namespace Portal.Applications.Events
{
    /// <summary>
    /// 表示同步事件
    /// </summary>
    public class SynchronizationEvent : ApplicationEvent
    {
        /// <summary>
        /// 修改Id
        /// </summary>
        public string ModifiedId
        {
            get;
            set;
        }

        /// <summary>
        /// 修改类型
        /// </summary>
        public ModifiedType Type
        {
            get;
            protected set;
        }
    }
}
