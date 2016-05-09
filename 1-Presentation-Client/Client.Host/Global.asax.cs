using System.Web.Mvc;
using System.Web.Routing;

namespace Client.Host
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            Portal.Client.FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            Portal.Client.RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
    }
}
