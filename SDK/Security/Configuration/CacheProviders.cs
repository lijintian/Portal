using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Threading.Tasks;

namespace Portal.SDK.Security.Configuration
{
    /// <summary>
    /// 表示缓存提供配置节
    /// </summary>
    public class CacheProviders : ConfigurationSection
    {
        [ConfigurationProperty("", IsDefaultCollection = true)]
        public ProviderCollection All
        {
            get
            {
                return (ProviderCollection)base[""];
            }
        }  
    }
}
