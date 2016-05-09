using System;
using System.Net;
using System.Web;
using Portal.Dto.Common;
using RestSharp;

namespace Portal.SDK.Common
{
    public static class Helpers
    {
        /// <summary>
        /// 生成错误信息
        /// </summary>
        /// <param name="response">响应信息</param>
        /// <param name="requestUri">请求的Uri</param>
        /// <returns>错误信息</returns>
        public static ErrorData GenerateErrorData(IRestResponse response, Uri requestUri)
        {
            var errorData = new ErrorData()
            {
                ErrorCode = response.StatusCode.ToString(),
                Message = response.Content,
                DateTime = DateTime.UtcNow,
                RequestUri = requestUri
            };

            return errorData;
        }

        /// <summary>
        /// 获取返回信息
        /// </summary>
        public static T GetResultFromRequest<T>(string uri, Method method) where T : ResponseBase, new()
        {
            var request = HttpClientFactory.CreateRequest(uri, method);
            return GetResultFromRequest<T>(request);
        }

        /// <summary>
        /// 获取返回信息
        /// </summary>
        public static T GetResultFromRequest<T>(string uri, object parameter, Method method) where T : ResponseBase, new()
        {
            uri = WebHelper.GetUrl(uri, parameter);
            var request = HttpClientFactory.CreateRequest(uri, method);
            return GetResultFromRequest<T>(request);
        }

        /// <summary>
        /// 获取返回信息
        /// </summary>
        /// <typeparam name="T">继承ResponseBase的对象</typeparam>
        /// <param name="request">请求信息</param>
        public static T GetResultFromRequest<T>(RestRequest request) where T : ResponseBase, new()
        {
            var client = HttpClientFactory.Create(Constants.PortalWebAddressKey);
            Uri requestUri = client.BuildUri(request);

            var res = client.Execute<T>(request);
            var response = res.Data;
            if (res.StatusCode == HttpStatusCode.OK && res.ErrorException == null)
            {
                response = res.Data;
            }
            else
            {
                response = response ?? new T();
                response.ErrorData = GenerateErrorData(res, requestUri);
            }

            return response;
        }

        public static T GetResultFromRequest<T>(RestRequest request, string baseUrl) where T : ResponseBase, new()
        {
            var client = HttpClientFactory.Create(baseUrl);
            var response = client.Execute<T>(request);

            var statsCode = (int)response.StatusCode;
            if (200 <= statsCode && statsCode < 300)
            {
                return response.Data;
            }
            else
            {
                throw new HttpException((int)response.StatusCode, response.Content);
            }
        }
    }
}
