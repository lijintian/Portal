using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace Portal.SDK.Security.Configuration
{
    /// <summary>
    /// 表示Portal验证基本配置节
    /// </summary>
    public class AuthenticationBase : ConfigurationSection
    {
        [ConfigurationProperty("application", IsRequired = false, DefaultValue = "App")]
        public string ApplicationName
        {
            get
            {
                return (string)base["application"];
            }
            set
            {
                base["application"] = value;
            }
        }

        [ConfigurationProperty("authCookieName", IsRequired = false, DefaultValue = ".CK1PortalAuth")]
        public string AuthCookieName
        {
            get
            {
                return (string)base["authCookieName"];
            }
            set
            {
                base["authCookieName"] = value;
            }
        }

        [ConfigurationProperty("authCookieDomain", IsRequired = false, DefaultValue = "")]
        public string AuthCookieDomain
        {
            get
            {
                return (string)base["authCookieDomain"];
            }
            set
            {
                base["authCookieDomain"] = value;
            }
        }

        [ConfigurationProperty("loginUrl", IsRequired = true)]
        public string LoginUrl
        {
            get
            {
                return (string)base["loginUrl"];
            }
            set
            {
                base["loginUrl"] = value;
            }
        }

        [ConfigurationProperty("cacheExpiredTime", IsRequired = false, DefaultValue = "60")]
        public int CacheExpiredTime
        {
            get
            {
                return (int)base["cacheExpiredTime"];
            }
            set
            {
                base["cacheExpiredTime"] = value;
            }
        }

        [ConfigurationProperty("cacheName", IsRequired = false)]
        public string CacheName
        {
            get
            {
                return (string)base["cacheName"];
            }
            set
            {
                base["cacheName"] = value;
            }
        }

        public override string ToString()
        {
            return string.Format("app={0}, cookie={1}, expiredTime={2}, cache={3}, loginurl={4}", 
                this.ApplicationName, this.AuthCookieName, 
                this.CacheExpiredTime, this.CacheName,
                this.LoginUrl);
        }
    }
}
