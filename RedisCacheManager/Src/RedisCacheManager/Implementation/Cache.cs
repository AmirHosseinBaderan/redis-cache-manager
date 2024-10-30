
namespace RedisCacheManager.Implementation;

public class Cache(ICacheBase cacheBase) : ICache
{
    public async Task<TModel?> GetItemAsync<TModel>(string key)
    {
        try
        {
            RedisValue value = await cacheBase.GetItemAsync(key);
            if (value.IsNullOrEmpty)
                return default;
            return JsonConvert.DeserializeObject<TModel>(value.ToString());
        }
        catch
        {
            return default;
        }
    }

    public async Task<TModel?> GetOrderSetItemAsync<TModel>(string key, Func<Task<TModel>> func)
    {
        try
        {
            RedisValue value = await cacheBase.GetOrderSetItemAsync(key, async () =>
            {
                TModel? res = await func();
                if (res is null)
                    return RedisValue.Null;
                string json = JsonConvert.SerializeObject(res);
                return new(json);
            });
            return value.IsNullOrEmpty
                ? await func()
                : JsonConvert.DeserializeObject<TModel>(value.ToString());
        }
        catch
        {
            return await func();
        }
    }

    public async Task<TModel?> GetOrderSetItemAsync<TModel>(string key, CacheDuration cacheDuration, Func<Task<TModel>> func)
    {
        try
        {
            RedisValue value = await cacheBase.GetOrderSetItemAsync(key, cacheDuration, async () =>
            {
                TModel? res = await func();
                if (res is null)
                    return RedisValue.Null;
                string json = JsonConvert.SerializeObject(res);
                return new(json);
            });
            return value.IsNullOrEmpty
                ? await func()
                : JsonConvert.DeserializeObject<TModel>(value.ToString());
        }
        catch
        {
            return await func();
        }
    }

    public async Task RemoveItemAsync(string key)
        => await cacheBase.RemoveItemAsync(key);

    public async Task<TModel?> SetItemAsync<TModel>(string key, TModel? obj)
        => await SetItemAsync(key, obj, cacheTime: null);

    public async Task<TModel?> SetItemAsync<TModel>(string key, TModel? obj, CacheDuration duration)
        => await SetItemAsync(key, obj, duration.ToTimeSpan());

    public async Task<TModel?> SetItemAsync<TModel>(string key, TModel obj, TimeSpan? cacheTime)
    {
        try
        {
            string json = JsonConvert.SerializeObject(obj);
            await cacheBase.SetItemAsync(key, new(json), cacheTime);
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
