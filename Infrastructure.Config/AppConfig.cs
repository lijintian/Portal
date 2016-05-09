using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace Portal.Infrastructure.Config
{
    /// <summary>
    /// 表示应用配置
    /// </summary>
    public static class AppConfig
    {
        /// <summary>
        /// 授权码过期时间, 单位为分钟
        /// </summary>
        public readonly static int AuthorizationCodeExpiredTime = 
            int.Parse(ConfigurationManager.AppSettings["AuthorizationCodeExpiredTime"] ?? "10");

        /// <summary>
        /// 回调地址只允许Https
        /// </summary>
        public static readonly bool CallbackUrlSecureHttps =
            bool.Parse(ConfigurationManager.AppSettings["CallbackUrlSecureHttps"] ?? "false");
    }
}
