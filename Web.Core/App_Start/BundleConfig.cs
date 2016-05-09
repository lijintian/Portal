using System.Web.Optimization;

namespace Portal.Web.Core
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/bundles/css").Include(
                      "~/Content/css/template.css"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                "~/Scripts/jquery.unobtrusive*",
                "~/Scripts/jquery.validate*"));
            bundles.Add(new ScriptBundle("~/bundles/bootstrap-jqueryval-mvc").Include(
                "~/Scripts/bootstrap.jquery.validate.mvc.js"));

            bundles.Add(new StyleBundle("~/bundles/css/login")
              .Include("~/Content/Css/bootstrap.css",
                  "~/Content/Css/bootstrap-responsive.css",
                  "~/Content/Css/login.css"));

            bundles.Add(new ScriptBundle("~/bundles/js/login")
                .Include("~/Scripts/bootstrap.jquery.validate.mvc.js",
                "~/Scripts/jquery.unobtrusive*",
                "~/Scripts/jquery.validate*",
                "~/Scripts/bootstrap*",
                "~/Scripts/jquery-{version}.js"));
        }
    }
}
