namespace MasterService.Service.Interface
{
    public interface ICacheService
    {
        Task<T?> GetOrCreateAsync<T>( string cacheKey,Func<Task<T>> retrieveDataFunc,TimeSpan? slidingExpiration = null);
        Task Clear(string? key);
        Task<List<string>> GetAllKeys(); 
    }
}