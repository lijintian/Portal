using RestSharp.Serializers;

namespace Portal.SDK.Common
{
    /// <summary>
    /// 基于ServiceStack执行序列化的操作类
    /// </summary>
    public class ServiceStackJsonSerializer : ISerializer
    {
        public ServiceStackJsonSerializer()
        {
            this.ContentType = "application/json";
        }

        public string RootElement { get; set; }

        public string Namespace { get; set; }

        public string DateFormat { get; set; }

        public string ContentType { get; set; }


        public string Serialize(object obj)
        {
            return ServiceStack.Text.JsonSerializer.SerializeToString(obj);
        }
    }
}
