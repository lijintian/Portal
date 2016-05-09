using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Portal.Infrastructure.Exceptions;
using EasyDDD.Infrastructure.Crosscutting.Logging;

namespace Portal.Web.Core.Extends
{
    /// <summary>
    /// 表示portal统一错误处理
    /// </summary>
    public class PortalHandleErrorAttribute : HandleErrorAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {
            if (filterContext.Exception != null)
            {
                if (!filterContext.ExceptionHandled && !(filterContext.Exception is HttpRequestValidationException) && !(filterContext.Exception is PortalException))
                {
                    Elmah.ErrorSignal.FromCurrentContext().Raise(filterContext.Exception, HttpContext.Current);
                }
            }
            this.LogError(filterContext.HttpContext, filterContext.Exception);

            this.CustomerHandleError(filterContext);

            //handle statuscode 500
            base.OnException(filterContext);

        }

        private void CustomerHandleError(ExceptionContext filterContext)
        {
            var stateCode = new HttpException(null, filterContext.Exception).GetHttpCode();
            switch (stateCode)
            {
                case 403:
                    filterContext.Result = new RedirectResult(HostUtility.Config.NoPermissionPage);
                    filterContext.ExceptionHandled = true;
                    break;
                case 404:
                    filterContext.Result = new RedirectResult(HostUtility.Config.NotFoundPage);
                    filterContext.ExceptionHandled = true;
                    break;
            }
            if (filterContext.Exception is HttpRequestValidationException)
            {
                filterContext.Exception = new HttpException(500, "请您输入合法字符串！");
            }
            //other error, handle as 500
            else if (stateCode != 500)
            {
                filterContext.Exception = new HttpException(500, "internal unhandle error.", filterContext.Exception);
            }
        }

        private void LogError(HttpContextBase context, Exception ex)
        {
            Task.Run(() => Log<ContentNegotiatedExceptionHandler>.LogError(this.ConvertToString(new Error(context)), ex));
        }

        private string ConvertToString(Error error)
        {
            StringBuilder builder = new StringBuilder();

            builder.AppendLine();
            builder.AppendFormat("Users:{0}", error.User).AppendLine();
            builder.AppendFormat("RequestUrl:{0}", error.RequestUrl).AppendLine();
            builder.AppendFormat("UrlReferrer:{0}", error.UrlReferrer).AppendLine();
            builder.AppendFormat("StatusCode: {0}", error.StatusCode).AppendLine();

            builder.Append("Cookies:");
            this.AppendCollection(builder, error.Cookies);

            builder.Append("Forms:");
            this.AppendCollection(builder, error.Forms);

            builder.Append("QueryString:");
            this.AppendCollection(builder, error.QueryString);

            builder.Append("ServerVariables:");
            this.AppendCollection(builder, error.ServerVariables);

            return builder.ToString();
        }

        private void AppendCollection(StringBuilder builder, NameValueCollection collection)
        {
            foreach (string key in collection.Keys)
            {
                var values = collection.GetValues(key);
                builder.AppendFormat("name={0}, values=", key);
                if (values != null)
                {
                    string[] arr = values;
                    for (int i = 0; i < arr.Length; i++)
                    {
                        builder.AppendFormat("[{0}],", arr[i]);
                    }
                }
                else
                {
                    builder.Append(",");
                }

            }
            builder.AppendLine();
        }
    }
}
