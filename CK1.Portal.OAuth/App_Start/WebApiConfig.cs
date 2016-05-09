using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using Portal.Web.Core.Extends;

namespace Portal.OAuth.App_Start
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
                routeTemplate: "{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional },
                constraints: new { controller = "OAuth2", action = "token" }
            );

            config.Routes.MapHttpRoute(
               name: "DefaultApi2",
               routeTemplate: "{controller}/{action}/{id}",
               defaults: new { id = RouteParameter.Optional },
               constraints: new { controller = "OAuth2", action = "tokenauth" }
           );
        }
    }
}