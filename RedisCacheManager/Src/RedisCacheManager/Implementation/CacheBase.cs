

namespace RedisCacheManager.Implementation;

internal class CacheBase(ICacheDb cacheDb) : ICacheBase
{
    public async ValueTask DisposeAsync()
    {
        GC.SuppressFinalize(this);
        await cacheDb.DisposeAsync();
    }

    public async Task<RedisValue> GetItemAsync(string key)
    {
        try
        {
            IDatabase? db = await cacheDb.GetDataBaseAsync();
            return db is null
                ? RedisValue.Null
                : await db.StringGetAsync(key);
        }
        catch
        {
            return RedisValue.Null;
        }
    }

    public async Task<RedisValue> GetOrderSetItemAsync(string key, Func<Task<RedisValue>> action)
    {
        try
        {
            IDatabase? db = await cacheDb.GetDataBaseAsync();
            if (db is null)
                return await action();

            RedisValue value = await db.StringGetAsync(key);
            if (value.IsNullOrEmpty)
            {
                var res = await action();
                return await SetItemAsync(key, res);
            }
            return value;
        }
        catch
        {
            return await action();
        }
    }

    public async Task<RedisValue> GetOrderSetItemAsync(string key, CacheDuration cacheDuration, Func<Task<RedisValue>> action)
    {
        try
        {
            IDatabase? db = await cacheDb.GetDataBaseAsync();
            if (db is null)
                return await action();

            RedisValue value = await db.StringGetAsync(key);
            if (value.IsNullOrEmpty)
            {
                var res = await action();
                return await SetItemAsync(key, res, cacheDuration.ToTimeSpan());
            }
            return value;
        }
        catch
        {
            return await action();
        }
    }

    public async Task RemoveItemAsync(string key)
    {
        try
        {
            IDatabase? db = await cacheDb.GetDataBaseAsync();
            if (db is null)
                return;

            await db.StringGetDeleteAsync(key);
        }
        catch
        {
            return;
        }
    }

    public async Task<RedisValue> SetItemAsync(string key, RedisValue? obj)
        => await SetItemAsync(key, obj, cacheTime: null);

    public async Task<RedisValue> SetItemAsync(string key, RedisValue? obj, CacheDuration duration)
        => await SetItemAsync(key, obj, duration.ToTimeSpan());

    public async Task<RedisValue> SetItemAsync(string key, RedisValue? obj, TimeSpan? cacheTime)
    {
        try
        {
            IDatabase? db = await cacheDb.GetDataBaseAsync();
            if (db is null || obj is null)
                return obj ?? RedisValue.Null;

            await db.StringSetAsync(key, (RedisValue)obj, cacheTime);
            return obj ?? RedisValue.Null;
        }
        catch
        {
            return obj ?? RedisValue.Null;
        }
    }

    public async Task<RedisValue> SetItemIfAsync(bool condition, string key, RedisValue? obj)
          => condition ?
          await SetItemAsync(key, obj, cacheTime: null)
        : obj ?? RedisValue.Null;

    public async Task<RedisValue> SetItemIfAsync(bool condition, string key, RedisValue? obj, CacheDuration duration)
         => condition ?
          await SetItemAsync(key, obj, cacheTime: duration.ToTimeSpan())
        : obj ?? RedisValue.Null;
}
