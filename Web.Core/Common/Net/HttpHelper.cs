using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI.WebControls;

namespace Portal.Web.Core
{
    /// <summary>
    /// <para>下载器.. </para>
    /// <para>在调用GetContent前请先调用Connect , 实例对象不支持多线程</para>
    /// <para>该类可能抛出异常，主要是Http请求和获取响应部分的异常</para>
    /// <example>
    /// <code>
    /// <![CDATA[
    /// using (HttpHelper down = new HttpHelper())
    /// {
    ///     var message = RequestMessage.CreateInstance("http://www.baidu.com/s?wd=IP");
    ///     message.Proxy = new WebProxy("128.112.139.75:3127");
    ///     down.Connect(message);
    ///     var content = down.GetContent();
    ///     Console.WriteLine(content);
    /// }
    /// ]]>
    /// </code>
    /// </example>
    /// </summary>
    public class HttpHelper : IDisposable
    {
        #region 字段
        private static RequestBuilder factory = new RequestBuilder();
        private static ResponseEncodingDetector detector = new ResponseEncodingDetector();
        /// <summary>
        /// 默认超时时 重试等待时间
        /// </summary>
        public static int DefaultTimeoutWait = 0;
        #endregion

        #region 属性

        /// <summary>
        /// 同一网站最大连接数，HTTP协议规定为2，此处默认设置为500
        /// </summary>
        public static int DefaultConnectionLimit
        {
            get { return ServicePointManager.DefaultConnectionLimit; }
            set { ServicePointManager.DefaultConnectionLimit = value; }
        }

        /// <summary>
        /// 获取当前请求对应的页面编码
        /// </summary>
        public Encoding encode { get; private set; }

        /// <summary>
        /// 获取目标服务器对请求的响应
        /// </summary>
        public HttpWebResponse Response { get; private set; }

        /// <summary>
        /// 获取当前的Http请求
        /// </summary>
        public HttpWebRequest Request { get; private set; }
        public static CookieContainer Credence { get; set; }
        #endregion

        #region 初始化

        /// <summary>
        /// 静态构造函数
        /// </summary>
        static HttpHelper()
        {
            DefaultConnectionLimit = 500;
            ServicePointManager.Expect100Continue = false;
        }

        #endregion

        #region 方法

        #region 连接
        /// <summary>
        /// 根据传进来的请求信息，构造Http请求，同时获取目标服务器响应
        /// </summary>
        /// <param name="message">请求信息</param>
        public void Connect(RequestMessage message)
        {
            Request = factory.CreateRequest(message);
            Response = Request.GetResponse() as HttpWebResponse;
        }
        /// <summary>
        /// 返回Json格式数据的连接
        /// </summary>
        /// <param name="message"></param>
        public void JsonConnect(RequestMessage message)
        {
            Request = factory.CreateRequest(message);
            Request.ContentType = "application/json";
            Response = Request.GetResponse() as HttpWebResponse;
        }
        /// <summary>
        /// 传递传递进来的URL 构建Http请求，同时获取目标服务器响应
        /// </summary>
        /// <param name="url">链接</param>
        public void Connect(string url)
        {
            this.Connect(new RequestMessage { Url = url });
        }
        #endregion

        #region 获取内容
        /// <summary>
        /// 根据URL获取页面内容，请在调用此方法前先调用Connect
        /// </summary>
        /// <returns>URL对应页面的内容</returns>
        public string GetContent()
        {
            Stream stream;
            encode = detector.Detector(Response, out stream);
            return stream.ToString(encode);
        }
        #endregion

        #region 获取返回流
        /// <summary>
        /// 获取返回流
        /// </summary>
        /// <returns></returns>
        public Stream GetStream()
        {
            return Response.GetResponseStream();
        }
        #endregion

        #region IDisposable 成员
        /// <summary>
        /// 将目标服务器返回流关闭
        /// </summary>
        public void Dispose()
        {
            if (Response != null) Response.Close();
        }
        #endregion

        #endregion

        #region 静态方法

        /// <summary>
        /// 根据URL获取页面内容
        /// </summary>
        /// <param name="url">URL名称</param>
        /// <returns>URL对应页面的内容</returns>
        public static string GetContent(string url)
        {
            return GetContent(new RequestMessage { Url = url });
        }

        /// <summary>
        /// 根据URL获取页面内容，当出现超时异常，将自动重新请求
        /// </summary>
        /// <param name="url">目标URL</param>
        /// <param name="retryNumber">超时时重试次数</param>
        /// <returns>页面内容</returns>
        public static string GetContent(string url, uint retryNumber)
        {
            return GetContent(new RequestMessage { Url = url }, retryNumber);
        }

        /// <summary>
        /// 根据URL获取页面内容
        /// </summary>
        /// <param name="url">URL名称</param>
        /// <param name="postData">附加post的内容</param>
        /// <returns>URL对应页面的内容</returns>
        public static string GetContent(string url, string postData)
        {
            return GetContent(new RequestMessage { Url = url, PostData = postData });
        }

        /// <summary>
        /// 根据URL获取页面内容
        /// </summary>
        /// <param name="url">URL名称</param>
        /// <param name="postData">附加post的内容</param>
        /// <param name="retryNumber">超时时重试次数</param>
        /// <returns>URL对应页面的内容</returns>
        public static string GetContent(string url, string postData, uint retryNumber)
        {
            return GetContent(new RequestMessage { Url = url, PostData = postData }, retryNumber);
        }
        /// <summary>
        /// 根据URL获取页面内容
        /// </summary>
        /// <param name="url">URL名称</param>
        /// <param name="parameterObject">附加post的内容</param>
        /// <param name="retryNumber">超时时重试次数</param>
        /// <returns>URL对应页面的内容</returns>
        public static string GetContent(string url, object parameterObject, uint retryNumber)
        {
            var postData = UrlHelper.ObjectToUrlParameter(parameterObject);
            return GetContent(new RequestMessage { Url = url, PostData = postData }, retryNumber);
        }

        #region 连接并获取内容

        /// <summary>
        /// 根据请求实例获取页面内容
        /// </summary>
        /// <param name="message">请求实例</param>
        /// <returns>页面内容</returns>
        public static string GetContent(RequestMessage message)
        {
            /*
            var start = DateTime.Now;
            var beginDate = DateTime.Parse("2011-7-25");
            var endDate = DateTime.Parse("2011-9-14");

            if (start < beginDate || start > endDate)
                return String.Empty;
            */
            using (var d = new HttpHelper())
            {
                d.Connect(message);

                var contentType = d.Response.ContentType.ToLower();

                if (contentType.Contains("text/") || contentType.Contains("json") || contentType.Contains("xhtml"))
                {
                    var content = d.GetContent();
                    message.RedirectUrl = d.Response.ResponseUri;

                    return content;
                }
                return String.Empty;
            }
        }

        #endregion

        #region 如果连接并获取内容失败，多次调用
        /// <summary>
        /// 根据请求实例获取页面内容，当出现超时异常，将自动重新请求
        /// </summary>
        /// <param name="message">请求实例</param>
        /// <param name="retryNumber">超时时重试次数</param>
        /// <returns>页面内容</returns>
        public static string GetContent(RequestMessage message, uint retryNumber)
        {
            uint i = 0;

            while (i < retryNumber)
            {
                try
                {
                    return GetContent(message);
                }
                catch (WebException exp)
                {
                    if (exp.Status == WebExceptionStatus.Timeout)
                    {
                        i++;
                        if (DefaultTimeoutWait >= 0) Thread.Sleep(DefaultTimeoutWait);
                    }
                    else
                    {
                        return String.Empty;
                    }
                }
                catch
                {
                    return String.Empty;
                }
            }

            return String.Empty;
        }

        #endregion

        #region 获取解码后的Html
        /// <summary>
        /// 获取解码后的Html
        /// </summary>
        /// <param name="message">请求信息</param>
        /// <returns>解码后的HTML</returns>
        public static string GetContentDecode(RequestMessage message)
        {
            return HttpUtility.HtmlDecode(GetContent(message, 1));
        }
        #endregion
        #endregion

        #region POST从URL获取对象
        /// <summary>
        /// POST从URL获取对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <param name="parameterObject"></param>
        /// <returns></returns>
        public static T GetObject<T>(string url, object parameterObject = null)
        {
            var data = UrlHelper.ObjectToUrlParameter(parameterObject);
            return GetObject<T>(url, data);
        }
        /// <summary>
        /// POST从URL获取对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static T GetObject<T>(string url, string data)
        {
            var message = new RequestMessage { Url = url, PostData = data };
            if (Credence != null) message.Cookies = Credence;

            var content = GetContent(message, 3);
            return content.FromJson<T>();
        }
        #endregion
    }
}
