using System.Web.Http;
using System.Web.Mvc;
using Portal.Web.Admin;
using MvcUnityDependencyResolver = Microsoft.Practices.Unity.Mvc.UnityDependencyResolver;
using WebApiUnityDependencyResolver = Microsoft.Practices.Unity.WebApi.UnityDependencyResolver;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(UnityWebApiActivator), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethod(typeof(UnityWebApiActivator), "Shutdown")]


namespace Portal.Web.Admin
{
    /// <summary>Provides the bootstrapping for integrating Unity with WebApi when it is hosted in ASP.NET</summary>
    public static class UnityWebApiActivator
    {
        private static EasyDDD.Infrastructure.Crosscutting.Ioc.Unity.UnityDependencyResolver _unityDependencyResolver;

        /// <summary>Integrates Unity when the application starts.</summary>
        public static void Start()
        {

            _unityDependencyResolver = new EasyDDD.Infrastructure.Crosscutting.Ioc.Unity.UnityDependencyResolver();
            EasyDDD.Infrastructure.Crosscutting.InversionOfControl.IoC.Initialize(_unityDependencyResolver);

            var container = _unityDependencyResolver.UnityContainer;
            var mvcResolver = new MvcUnityDependencyResolver(container);
            var webApiResolver = new WebApiUnityDependencyResolver(container);

            //支持MVC IoC
            DependencyResolver.SetResolver(mvcResolver);
            //支持WebApi Ioc
            GlobalConfiguration.Configuration.DependencyResolver = webApiResolver;
        }

        /// <summary>Disposes the Unity container when the application is shut down.</summary>
        public static void Shutdown()
        {
            var container = _unityDependencyResolver == null ? null : _unityDependencyResolver.UnityContainer;
            if (container != null) container.Dispose();
        }
    }
}
