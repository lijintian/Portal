using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using Portal.Web.Core.Extends;

namespace Portal.Web.Core
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();
            //全局异常处理器
            config.Services.Replace(typeof(IExceptionHandler), new ContentNegotiatedExceptionHandler());

            //替换WebAPI默认的Json.net JsonFormatter为自定义JsonFormatter
            config.Formatters.Clear();
            config.Formatters.Add(new CustomJsonFormatter());

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
