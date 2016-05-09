using RestSharp.Deserializers;

namespace Portal.SDK.Common
{
    /// <summary>
    /// 基于ServiceStack执行反序列化的操作类
    /// </summary>
    public class ServiceStackJsonDeserializer : IDeserializer
    {
        public T Deserialize<T>(RestSharp.IRestResponse response)
        {
            var result = ServiceStack.Text.JsonSerializer.DeserializeFromString<T>(response.Content);
            return result;
        }

        public T Deserialize<T>(string message)
        {
            var result = ServiceStack.Text.JsonSerializer.DeserializeFromString<T>(message);
            return result;
        }

        public string DateFormat { get; set; }

        public string Namespace { get; set; }

        public string RootElement { get; set; }
    }
}
