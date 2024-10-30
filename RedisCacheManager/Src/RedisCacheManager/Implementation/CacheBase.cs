

namespace RedisCacheManager.Implementation;

internal class CacheBase(ICacheDb cacheDb) : ICacheBase
{
    public async ValueTask DisposeAsync()
    {
        GC.SuppressFinalize(this);
        await cacheDb.DisposeAsync();
    }

    public Task<RedisValue?> GetItemAsync(string key)
    {
        throw new NotImplementedException();
    }

    public Task<RedisValue?> GetOrderSetItemAsync(string key, Func<Task<RedisValue>> action)
    {
        throw new NotImplementedException();
    }

    public Task<RedisValue?> GetOrderSetItemAsync(string key, CacheDuration cacheDuration, Func<Task<RedisValue>> action)
    {
        throw new NotImplementedException();
    }

    public Task RemoveItemAsync(string key)
    {
        throw new NotImplementedException();
    }

    public Task<RedisValue?> SetItemAsync(string key, RedisValue? obj)
    {
        throw new NotImplementedException();
    }

    public Task<RedisValue?> SetItemAsync(string key, RedisValue? obj, CacheDuration duration)
    {
        throw new NotImplementedException();
    }

    public Task<RedisValue?> SetItemAsync(string key, RedisValue obj, TimeSpan? cacheTime)
    {
        throw new NotImplementedException();
    }

    public Task<RedisValue?> SetItemIfAsync(bool condition, string key, RedisValue? obj)
    {
        throw new NotImplementedException();
    }

    public Task<RedisValue?> SetItemIfAsync(bool condition, string key, RedisValue? obj, CacheDuration duration)
    {
        throw new NotImplementedException();
    }
}
