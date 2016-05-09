using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace Portal.SDK.Security.Configuration
{
    /// <summary>
    /// 表示portal验证配置组
    /// </summary>
    public class PortalAuthentication : ConfigurationSectionGroup
    {
        public AuthenticationBase AuthenticationBase
        {
            get
            {
                return (AuthenticationBase)base.Sections["Authentication"];
            }
        }

        public CacheProviders CacheProviders
        {
            get
            {
                return (CacheProviders)base.Sections["CacheProviders"];
            }
        }
    }
}
