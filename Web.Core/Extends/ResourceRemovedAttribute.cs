using System;
using System.Net;
using System.Net.Http;
using System.Web.Http.Filters;

namespace Portal.Web.Core.Extends
{
    /// <summary>
    /// 资源被移除不再有效
    /// </summary>
    public class ResourceRemovedAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            if (actionExecutedContext.Exception is ResourceRemovedException)
            {
                actionExecutedContext.Response = new HttpResponseMessage(HttpStatusCode.Gone);
            }
        }
    }
    public class ResourceRemovedException : Exception { }
}
