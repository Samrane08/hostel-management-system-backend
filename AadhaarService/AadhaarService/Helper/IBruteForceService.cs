using Microsoft.Extensions.Caching.Memory;

namespace AadhaarService.Helper
{
    public interface IBruteForceService
    {
        Task<bool> IsBruteForce(string ipAddress);
        void RegisterFailedAttempt(string ipAddress);
        
    }
    public class BruteForceService : IBruteForceService
    {
        private readonly IMemoryCache _cache;
        private const int MaxAttempts = 5;    // Maximum allowed failed attempts
        private const int TimeWindowInMinutes = 5;  // Time window in minutes for tracking failed attempts

        public BruteForceService(IMemoryCache cache)
        {
            _cache = cache;
        }

        // Method to check for brute force attempts based on the client's IP address
        public async Task<bool> IsBruteForce(string ipAddress)
        {
            var cacheKey = $"Aadhar_{ipAddress}";
            var attempts = _cache.Get<int>(cacheKey);

            if (attempts == 0)
            {
               attempts++;
            }

            if (attempts >= MaxAttempts)
            {
                return true; // Brute force detected, deny access
            }

            return false;
        }

        // Method to register a failed attempt
        public void RegisterFailedAttempt(string ipAddress)
        {
            var cacheKey = $"Aadhar_{ipAddress}";

            // Increment the failed attempt count in cache
            var attempts = _cache.Get<int>(cacheKey);
            _cache.Set(cacheKey, attempts + 1, TimeSpan.FromMinutes(TimeWindowInMinutes));
        }

    }
}
