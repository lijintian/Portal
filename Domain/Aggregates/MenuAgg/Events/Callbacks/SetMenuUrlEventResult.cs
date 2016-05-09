

using EasyDDD.Core.Event;

namespace Portal.Domain.Aggregates.MenuAgg.Events.Callbacks
{
    public class SetMenuUrlEventResult: IDomainEventResult
    {
          /// <summary>
        /// 是否存在ParentId
        /// </summary>
        public string Url { get; private set; }

        public SetMenuUrlEventResult(string url)
        {
            this.Url = url;
        }
    }
}
