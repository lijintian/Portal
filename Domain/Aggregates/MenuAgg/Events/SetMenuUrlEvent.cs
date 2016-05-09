

using EasyDDD.Core.Event;

namespace Portal.Domain.Aggregates.MenuAgg.Events
{
    public class SetMenuUrlEvent : DomainEvent
    {
        #region 属性
        /// <summary>
        /// 所属应用程序
        /// </summary>
        public string ApplicationId { get; set; }

        public string Url { get; set; }
        #endregion

        #region 初始化
        public SetMenuUrlEvent(string applicationId,string url)
        {
            this.Url = url;
            this.ApplicationId = applicationId;
        }
        #endregion
    }
}
