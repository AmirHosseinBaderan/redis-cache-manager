﻿namespace RedisCacheManager.Abstraction;

public interface ICacheBase : IAsyncDisposable
{
    Task<RedisValue> GetItemAsync(string key);

    Task<RedisValue> SetItemAsync(string key, RedisValue? obj);

    Task RemoveItemAsync(string key);

    Task<RedisValue> GetOrderSetItemAsync(string key, Func<Task<RedisValue>> action);

    Task<RedisValue> GetOrderSetItemAsync(string key, CacheDuration cacheDuration, Func<Task<RedisValue>> action);

    Task<RedisValue> SetItemAsync(string key, RedisValue? obj, TimeSpan? cacheTime);
}
