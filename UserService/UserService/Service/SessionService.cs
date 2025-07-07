using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using Repository.Interface;
using Service.Interface;
using UserService.Helper;

namespace UserService.Service
{ 
    // SessionService.cs
    public class SessionService : ISessionService
    {
        // private readonly IConnectionMultiplexer _redis;
        // private readonly IDistributedCache _cache;
        private readonly IMemoryCache _cacheService;

        //public SessionService(IConnectionMultiplexer redis, IDistributedCache cache, IMemoryCache cacheService)
        public SessionService(IMemoryCache cacheService)
        {
            //_redis = redis;
            //_cache = cache;
            _cacheService = cacheService;
        }

        //public async Task StoreSessionInRedis(string userId, string sessionId)
        //{
        //    var db = _redis.GetDatabase();
            
        //    var sessionData = await db.StringGetAsync(userId);
            
        //    if (sessionData.HasValue && sessionData != sessionId)
        //        db.KeyDelete(userId);

        //    await db.StringSetAsync(userId, sessionId, TimeSpan.FromMinutes(30)); // Set expiration to 1 hour
        //}

        //public async Task<string> GetSessionFromRedis(string userId)
        //{
        //    var db = _redis.GetDatabase();
        //    var sessionData = await db.StringGetAsync(userId);

        //    return sessionData.HasValue ? sessionData : "";
        //}
        //public async Task<bool> RemoveKey(string userId)
        //{
        //    var db = _redis.GetDatabase();
        //    return db.KeyDelete(userId);
        //}
        public async Task StoreSessionInMemoryCache(string userId, string sessionId)
        {
           var sessionData = _cacheService.Get(userId);

            if (sessionData != null && sessionData != sessionId)
                _cacheService.Remove(userId);

            _cacheService.Set(userId, sessionId, TimeSpan.FromMinutes(30)); // Set expiration to 1 hour
        }

        public async Task<string> GetSessionFromMemoryCache(string userId)
        {
            var sessionData = _cacheService.Get(userId);
            return sessionData != null ? sessionData.ToString(): "";
        }
        public async Task<bool> RemoveKeyMemoryCache(string userId)
        {
            _cacheService.Remove(userId);
            return  true;
        }
    }
}
