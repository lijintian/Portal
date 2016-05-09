using System;
using System.Configuration;

namespace Portal.SDK.Common
{
    /// <summary>
    /// 自定义HttpClient
    /// </summary>
    public class CustomHttpClient : RestSharp.RestClient
    {
        public CustomHttpClient(string name)
            : base(ConfigurationManager.AppSettings[name])
        {
            //使用ServiceStack序列化json
            this.AddHandler("application/json", new ServiceStackJsonDeserializer());
        }
    }
    
}
