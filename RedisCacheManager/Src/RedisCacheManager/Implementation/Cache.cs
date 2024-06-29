using Newtonsoft.Json;
using RedisCacheManager.Abstraction;
using RedisCacheManager.Configuration;
using RedisCacheManager.Core;
using StackExchange.Redis;

namespace RedisCacheManager.Implementation;

internal class Cache(ICacheDb cacheDb) : ICache
{
    public async ValueTask DisposeAsync()
    {
        GC.SuppressFinalize(this);
        await cacheDb.DisposeAsync();
    }

    public async Task<TModel?> GetItemAsync<TModel>(string key)
    {
        try
        {
            IDatabase? db = await cacheDb.GetDataBaseAsync();
            if (db is null)
                return default;

            RedisValue value = await db.StringGetAsync(key);
            if (value.IsNullOrEmpty)
                return default;
            return JsonConvert.DeserializeObject<TModel>(value.ToString());
        }
        catch
        {
            return default;
        }
    }

    public async Task<TModel?> GetOrderSetItemAsync<TModel>(string key, Func<TModel> func)
    {
        try
        {
            IDatabase? db = await cacheDb.GetDataBaseAsync();
            if (db is null)
                return func();

            RedisValue value = await db.StringGetAsync(key);
            if (value.IsNullOrEmpty)
            {
                var res = func();
                return await SetItemAsync(key, res);
            }
            return JsonConvert.DeserializeObject<TModel>(value.ToString());
        }
        catch
        {
            return func();
        }
    }

    public async Task<TModel?> GetOrderSetItemAsync<TModel>(string key, CacheDuration cacheDuration, Func<TModel> func)
    {
        try
        {
            IDatabase? db = await cacheDb.GetDataBaseAsync();
            if (db is null)
                return func();

            RedisValue value = await db.StringGetAsync(key);
            if (value.IsNullOrEmpty)
            {
                var res = func();
                return await SetItemAsync(key, res, cacheDuration.ToTimeSpan());
            }
            return JsonConvert.DeserializeObject<TModel>(value.ToString());
        }
        catch
        {
            return func();
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

    public async Task<TModel?> SetItemAsync<TModel>(string key, TModel? obj)
        => await SetItemAsync(key, obj, cacheTime: null);

    public async Task<TModel?> SetItemAsync<TModel>(string key, TModel? obj, CacheDuration duration)
        => await SetItemAsync(key, obj, duration.ToTimeSpan());

    public async Task<TModel?> SetItemAsync<TModel>(string key, TModel obj, TimeSpan? cacheTime)
    {
        try
        {
            IDatabase? db = await cacheDb.GetDataBaseAsync();
            if (db is null)
                return obj;

            string json = JsonConvert.SerializeObject(obj);
            await db.StringSetAsync(key, new(json), cacheTime);
            return obj;
        }
        catch
        {
            return obj;
        }
    }

    public async Task<TModel?> SetItemIfAsync<TModel>(bool condition, string key, TModel? obj)
        => condition ?
          await SetItemAsync(key, obj, cacheTime: null)
        : obj;

    public async Task<TModel?> SetItemIfAsync<TModel>(bool condition, string key, TModel? obj, CacheDuration duration)
        => condition ?
          await SetItemAsync(key, obj, cacheTime: duration.ToTimeSpan())
        : obj;
}
