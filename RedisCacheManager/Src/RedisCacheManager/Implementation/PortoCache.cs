using Google.Protobuf;
using Newtonsoft.Json;
using RedisCacheManager.Abstraction;
using RedisCacheManager.Configuration;
using RedisCacheManager.Core;
using StackExchange.Redis;

namespace RedisCacheManager.Implementation;

public class PortoCache(ICacheDb cacheDb) : IPortoCache
{
    public async ValueTask DisposeAsync()
    {
        GC.SuppressFinalize(this);
        await cacheDb.DisposeAsync();
    }

    public async Task<TModel?> GetItemAsync<TModel>(string key) where TModel : IMessage<TModel>
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

    public async Task<TModel?> GetOrderSetItemAsync<TModel>(string key, Func<TModel> func) where TModel : IMessage<TModel>
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

    public async Task<TModel?> GetOrderSetItemAsync<TModel>(string key, CacheDuration cacheDuration, Func<Task<TModel>> func) where TModel : IMessage<TModel>
    {
        try
        {
            IDatabase? db = await cacheDb.GetDataBaseAsync();
            if (db is null)
                return await func();

            RedisValue value = await db.StringGetAsync(key);
            if (value.IsNullOrEmpty)
            {
                TModel? res = await func();
                return await SetItemAsync(key, res, cacheDuration.ToTimeSpan());
            }
            return JsonConvert.DeserializeObject<TModel>(value.ToString());
        }
        catch
        {
            return await func();
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

    public async Task<TModel?> SetItemAsync<TModel>(string key, TModel? obj) where TModel : IMessage<TModel>
        => await SetItemAsync(key, obj, cacheTime: null);

    public async Task<TModel?> SetItemAsync<TModel>(string key, TModel? obj, CacheDuration duration) where TModel : IMessage<TModel>
        => await SetItemAsync(key, obj, duration.ToTimeSpan());

    public async Task<TModel?> SetItemAsync<TModel>(string key, TModel? obj, TimeSpan? cacheTime) where TModel : IMessage<TModel>
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

    public async Task<TModel?> SetItemIfAsync<TModel>(bool condition, string key, TModel? obj) where TModel : IMessage<TModel>
        => condition ?
          await SetItemAsync(key, obj, cacheTime: null)
        : obj;

    public async Task<TModel?> SetItemIfAsync<TModel>(bool condition, string key, TModel? obj, CacheDuration duration) where TModel : IMessage<TModel>
        => condition ?
          await SetItemAsync(key, obj, cacheTime: duration.ToTimeSpan())
        : obj;
}
