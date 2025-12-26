public interface ICacheService
{
    Task<T?> GetDataAsync<T>(string key);
    Task SetDataAsync<T>(string key, T value, TimeSpan? expirationTime = null);
    Task RemoveDataAsync(string key);
}