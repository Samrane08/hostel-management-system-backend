using MasterService.Service.Interface;
using Microsoft.Extensions.Caching.Memory;

namespace MasterService.Service.Implemetation
{
    public class CacheService : ICacheService
    {
        private readonly IMemoryCache _cache;
        private readonly List<string> _keys;
        public CacheService(IMemoryCache cache)
        {
            _cache = cache;
            _keys = new List<string>();
        }

        public async Task<T?> GetOrCreateAsync<T>(
        string cacheKey,
        Func<Task<T>> retrieveDataFunc,
        TimeSpan? slidingExpiration = null)
        {
            try
            {
                if (!_cache.TryGetValue(cacheKey, out T? cachedData))
                {
                    cachedData = await retrieveDataFunc();
                    _keys.Add(cacheKey);
                    var cacheEntryOptions = new MemoryCacheEntryOptions
                    {
                        SlidingExpiration = slidingExpiration ?? TimeSpan.FromDays(1)
                    };
                    _cache.Set(cacheKey, cachedData, cacheEntryOptions);
                }
                return cachedData;
            }
            catch (Exception)
            {
                return (T)Convert.ChangeType(null, typeof(T));
            }
        }

        public async Task Clear(string? key)
        {
            if (!string.IsNullOrWhiteSpace(key))
            {
                _cache.Remove(key);
            }
            else
            {
                foreach (var v in _keys)
                {
                    _cache.Remove(v);
                }
                _keys.Clear();
            }
        }

        public async Task<List<string>> GetAllKeys()
        {
            return _keys;
        }
    }
}
