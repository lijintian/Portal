using System.Web.Mvc;
using Portal.Web.Core.Extends;

namespace Portal.OAuth
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new PortalHandleErrorAttribute());
            //filters.Add(new AuthorizeAttribute());
        }
    }
}
