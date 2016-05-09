using EasyDDD.Core.Aggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Portal.Domain.Aggregates.SynchronizationAgg
{
    /// <summary>
    /// 表示用户信息，权限修改同步处理
    /// </summary>
    public class SynchronizationInfo : AggregateRoot
    {
        public SynchronizationInfo()
        {
            this.GenerateNewIdentity();
            this.UpdateOn = DateTime.Now;
        }

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
            set;
        }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime UpdateOn
        {
            get;
            set;
        }
    }
}
