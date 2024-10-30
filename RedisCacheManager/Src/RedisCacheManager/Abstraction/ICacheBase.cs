namespace RedisCacheManager.Abstraction;

internal interface ICacheBase : IAsyncDisposable
{
    Task<RedisValue?> GetItemAsync(string key);

    Task<RedisValue?> SetItemAsync(string key, RedisValue? obj);

    Task<RedisValue?> SetItemAsync(string key, RedisValue? obj, CacheDuration duration);

    Task<RedisValue?> SetItemIfAsync(bool condition, string key, RedisValue? obj);

    Task<RedisValue?> SetItemIfAsync(bool condition, string key, RedisValue? obj, CacheDuration duration);

    Task RemoveItemAsync(string key);

    Task<RedisValue?> GetOrderSetItemAsync(string key, Func<Task<RedisValue>> action);

    Task<RedisValue?> GetOrderSetItemAsync(string key, CacheDuration cacheDuration, Func<Task<RedisValue>> action);

    Task<RedisValue?> SetItemAsync(string key, RedisValue? obj, TimeSpan? cacheTime);
}
