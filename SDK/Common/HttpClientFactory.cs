using System;
using Portal.SDK.Core;
using Portal.SDK.Security;
using RestSharp;

namespace Portal.SDK.Common
{
    /// <summary>
    /// HttpClient创建工厂
    /// </summary>
    public static class HttpClientFactory
    {
        /// <summary>
        /// 创建RestClient对象
        /// </summary>
        /// <param name="baseAddressConfigeName">基地址配置名</param>
        /// <returns>RestClient对象</returns>
        public static RestClient Create(string baseAddressConfigeName)
        {
            return new CustomHttpClient(baseAddressConfigeName);
        }

        /// <summary>
        /// 创建RestRequest对象
        /// </summary>
        /// <param name="uri">地址信息</param>
        /// <param name="method">方法(Get,Post,Put,Delete)</param>
        /// <returns>RestRequest对象</returns>
        public static RestRequest CreateRequest(string uri, Method method)
        {
            string appName = WebHelper.GetAppName();
            if (!string.IsNullOrEmpty(appName))
            {
                uri = WebHelper.GetUrl(uri, Constants.ApplicationParamKey, CryptUtility.Encrypt(Constants.DecryptKey, appName));
            }
            var request = new RestRequest(uri, method)
            {
                RequestFormat = DataFormat.Json,
                JsonSerializer = new ServiceStackJsonSerializer()
            };
            return request;

        }

        /// <summary>
        /// 创建RestRequest对象
        /// </summary>
        /// <param name="uri">地址信息Uri对象</param>
        /// <param name="method">方法(Get,Post,Put,Delete)</param>
        /// <returns>RestRequest对象</returns>
        public static RestRequest CreateRequest(Uri uri, Method method)
        {
            var request = new RestRequest(uri, method)
            {
                RequestFormat = DataFormat.Json,
                JsonSerializer = new ServiceStackJsonSerializer()
            };
            return request;

        }
    }
}
