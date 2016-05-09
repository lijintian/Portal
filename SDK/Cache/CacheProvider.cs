using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Portal.SDK.Security;
using Portal.SDK.Security.Configuration;

namespace Portal.SDK.Cache
{
    static class CacheProvider
    {
        public static ICache Create()
        {
            if (string.IsNullOrEmpty(CK1PortalAuthenticationConfig.CacheName))
            {
                return InMemoryCache.Default;
            }
            else
            {
                var cacheType = CK1PortalAuthenticationConfig.CacheProviders[CK1PortalAuthenticationConfig.CacheName];
                if (string.IsNullOrEmpty(cacheType))
                {
                    throw new ArgumentException("missed cache type.");
                }

                return (ICache)Activator.CreateInstance(Type.GetType(cacheType));
            }
        }
    }
}
