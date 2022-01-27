using Microsoft.Extensions.Caching.Memory;
using Oxygen.MulitlevelCache;

namespace AppSimple.CacheImpl
{
    public class L1Cache : IL1CacheServiceFactory
    {
        private readonly IMemoryCache _cache;
        public L1Cache(IMemoryCache memoryCache)
        {
            this._cache = memoryCache;
        }
        public T Get<T>(string key)
        {
            var result = _cache.Get<T>(key);
            return result;
        }

        public bool Set<T>(string key, T value, int expireTimeSecond = 0)
        {
            return _cache.Set(key, value, DateTimeOffset.Now.AddSeconds(expireTimeSecond)) != null;
        }
    }
}
