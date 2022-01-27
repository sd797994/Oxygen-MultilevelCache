using CSRedis;
using Oxygen.MulitlevelCache;

namespace AppSimple.CacheImpl
{
    public static class cSRedisClientImpl
    {
        public static Lazy<CSRedisClient> cSRedisClient = new Lazy<CSRedisClient>(() => new CSRedisClient("127.0.0.1:6379,prefix=systemcacahe_"));
    }
    public class L2Cache : IL2CacheServiceFactory
    {
        public async Task<T> GetAsync<T>(string key)
        {
            var result = await cSRedisClientImpl.cSRedisClient.Value.GetAsync<T>(key);
            return result;
        }

        public async Task<bool> SetAsync<T>(string key, T value, int expireTimeSecond = 0)
        {
            return await cSRedisClientImpl.cSRedisClient.Value.SetAsync(key, value, expireTimeSecond);
        }
    }
}
