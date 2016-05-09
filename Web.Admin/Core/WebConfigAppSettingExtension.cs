using System.Web.Mvc;


namespace Portal.Web.Admin.Core
{
    public static class WebConfigAppSettingExtension
    {
        public static MvcHtmlString GetAppSetting(this HtmlHelper htmlHelper, AppSettingKey key)
        {
            return MvcHtmlString.Create(AppSettingHelper.Get(key));
        }
    }
}
