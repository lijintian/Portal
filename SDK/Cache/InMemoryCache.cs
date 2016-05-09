using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Caching;
using Portal.SDK.Security;

namespace Portal.SDK.Cache
{
    /// <summary>
    /// 表示内存中的缓存
    /// </summary>
    public class InMemoryCache : ICache
    {
        private static readonly MemoryCache MemoryCache = MemoryCache.Default;

        public InMemoryCache()
        {

        }

        public T Get<T>(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return default(T);
            }

            return (T)InMemoryCache.MemoryCache.Get(key);
        }

        public void Set(string key, object data, int cacheTimeInMinute)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentException("key");
            }
            if (data == null)
            {
                throw new ArgumentException("data");
            }

            var policy = new CacheItemPolicy()
            {
                SlidingExpiration = TimeSpan.FromMinutes(CK1PortalAuthenticationConfig.CacheExpiredTime),
                Priority = CacheItemPriority.NotRemovable
            };
            InMemoryCache.MemoryCache.Set(key, data, policy);
        }

        public bool IsSet(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return false;
            }

            return InMemoryCache.MemoryCache.Contains(key);
        }

        public void Remove(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentException("key");
            }

            InMemoryCache.MemoryCache.Remove(key);
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public readonly static InMemoryCache Default = new InMemoryCache();
    }
}
