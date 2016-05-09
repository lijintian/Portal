
using System;
using System.Configuration;
namespace Portal.Web.Admin.Core
{
   public class AppSettingHelper
    {
        public static string Get(string key)
        {
            var val = ConfigurationManager.AppSettings[key];
            if (string.IsNullOrEmpty(val))
                throw new InvalidOperationException(
                    string.Format("应用程序配置`{0}`未设置", key));
            return val;
        }

        public static string Get(AppSettingKey key)
        {
            return Get(key.ToString().Replace("_", ":")).Trim();
        }
    }
   public enum AppSettingKey
   {
       // ReSharper disable once InconsistentNaming
       Login_API_EXPIRETIME,

       // ReSharper disable once InconsistentNaming
       AMAZON_LOGIN_CLIENTID,

       // ReSharper disable once InconsistentNaming
       AMAZON_LOGIN_AUTHORIZE_URL,

       // ReSharper disable once InconsistentNaming
       Login_DEFAULT_Store_URL,

       Login_DEFAULT_Client_URL,
       // ReSharper disable once InconsistentNaming
       SSO_YEWU_WEBAPI_URL,

       // ReSharper disable once InconsistentNaming
       SSO_YEWU_WEBAPI_SECRET,

       // ReSharper disable once InconsistentNaming
       SSO_YEWU_WEBAPI_APPID,
   }

}
