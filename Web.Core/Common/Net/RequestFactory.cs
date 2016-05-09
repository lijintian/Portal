/*
 EasyDDD
系统名称：  基础类库
模块名称：  RequestBuilder HTTP请求构建类
创建日期：  2015-08-06

内容摘要： 
*/

using System;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Portal.Web.Core
{
    /// <summary>
    /// HttpWebRequest请求的构建类
    /// </summary>
    public class RequestBuilder
    {
        #region 01.根据传进来的请求参数返回响应对象

        /// <summary>
        /// 根据传进来的请求参数返回响应对象
        /// <para>可能引发异常，请注意处理</para>
        /// </summary>
        /// <param name="message">请求信息封装实体类</param>
        /// <returns>目标URL的响应对象</returns>
        public HttpWebRequest CreateRequest(RequestMessage message)
        {
            var request = InitializeRequest(message);

            if (String.IsNullOrEmpty(message.PostData))
                return CreateGetRequest(request);
            return CreatePostRequest(request, message.PostData);
        }

        #endregion

        #region 02.根据传进来的请求信息构造HttpWebRequest对象

        /// <summary>
        /// 根据传进来的请求信息构造HttpWebRequest对象
        /// </summary>
        /// <param name="message">请求信息</param>
        /// <returns>Http请求实例</returns>
        private HttpWebRequest InitializeRequest(RequestMessage message)
        {
            var request = HttpWebRequest.Create(message.Url) as HttpWebRequest;
            request.AllowAutoRedirect = true;
            request.KeepAlive = true;

            request.Accept = "*/*";
            request.Headers.Add("Accept-Language", "zh-cn");

            request.Headers["Accept-Encoding"] = "gzip, deflate";
            request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;

            request.UserAgent = message.UserAgent;
            request.Referer = message.RefererUrl;
            request.CookieContainer = message.Cookies;
            request.Timeout = message.TimeOut;

            if (message.Proxy != null) request.Proxy = message.Proxy;

            return request;
        }

        #endregion

        #region 03.构造Get类型的http请求实例

        /// <summary>
        /// 构造Get类型的http请求实例
        /// </summary>
        private HttpWebRequest CreateGetRequest(HttpWebRequest request)
        {
            request.Method = "GET";
            return request;
        }

        #endregion

        #region 04.构造post类型的http请求实例
        /// <summary>
        /// 构造post类型的http请求实例
        /// </summary>
        /// <param name="request">具有一定参数的http请求实例</param>
        /// <param name="data">想要post的数据</param>
        /// <returns>构造好的附带了post数据的http请求对象</returns>
        private HttpWebRequest CreatePostRequest(HttpWebRequest request, string data)
        {
            if (!string.IsNullOrEmpty(data))
            {
                var Data = Encoding.Default.GetBytes(data);
                request.Method = "POST";
                request.ContentLength = Data.Length;
                request.ContentType = "application/x-www-form-urlencoded";
                ServicePointManager.ServerCertificateValidationCallback =
                        delegate(Object obj, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
                        { return true; };
                using (Stream outStream = request.GetRequestStream())
                {
                    outStream.Write(Data, 0, Data.Length);
                }
            }
            else
            {
                request.ContentLength = 0;
            }
            return request;
        }

        #endregion

        #region 05.获取内容
        /// <summary>
        /// 获取内容
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string GetContent(string url)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                var resp = request.GetResponse();
                return new StreamReader(resp.GetResponseStream()).ReadToEnd();
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }
        #endregion
    }
}
