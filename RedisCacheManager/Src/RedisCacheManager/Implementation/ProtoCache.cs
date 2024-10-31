namespace RedisCacheManager.Implementation;

public class ProtoCache<TModel>(ICacheBase cacheBase) : ICache<TModel> where TModel : IMessage<TModel>, new()
{
    public async Task<TModel?> GetItemAsync(string key)
    {
        try
        {
            RedisValue value = await cacheBase.GetItemAsync(key);
            return value.IsNullOrEmpty
                ? default
                : ((byte[])value!).Deserialize<TModel>();
        }
        catch
        {
            return default;
        }
    }

    public async Task<TModel?> GetOrderSetItemAsync(string key, Func<Task<TModel>> func)
    {
        try
        {
            RedisValue value = await cacheBase.GetOrderSetItemAsync(key, async () =>
            {
                TModel? res = await func();
                return res is null
                ? RedisValue.Null
                : (RedisValue)res.Serialize();
            });
            return value.IsNullOrEmpty
                ? await func()
                : ((byte[])value!).Deserialize<TModel>();
        }
        catch
        {
            return await func();
        }
    }

    public async Task<TModel?> GetOrderSetItemAsync(string key, CacheDuration cacheDuration, Func<Task<TModel>> func)
    {
        try
        {
            RedisValue value = await cacheBase.GetOrderSetItemAsync(key, cacheDuration, async () =>
            {
                TModel? res = await func();
                return res is null
                ? RedisValue.Null
                : (RedisValue)res.Serialize();
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

    public async Task<TModel?> GetOrderSetItemAsync(string key, Func<TModel> action)
    {
        try
        {
            RedisValue value = await cacheBase.GetOrderSetItemAsync(key, () =>
            {
                TModel? res = action();
                return res is null
                ? RedisValue.Null
                : (RedisValue)res.Serialize();
            });
            return value.IsNullOrEmpty
                ? action()
                : ((byte[])value!).Deserialize<TModel>();
        }
        catch
        {
            return action();
        }
    }

    public async Task<TModel?> GetOrderSetItemAsync(string key, CacheDuration cacheDuration, Func<TModel> action)
    {
        try
        {
            RedisValue value = await cacheBase.GetOrderSetItemAsync(key, cacheDuration, () =>
            {
                TModel? res = action();
                return res is null
                ? RedisValue.Null
                : (RedisValue)res.Serialize();
            });
            return value.IsNullOrEmpty
                ? action()
                : JsonConvert.DeserializeObject<TModel>(value.ToString());
        }
        catch
        {
            return action();
        }
    }

    public async Task RemoveItemAsync(string key)
        => await cacheBase.RemoveItemAsync(key);

    public async Task<TModel?> SetItemAsync(string key, TModel? obj)
        => await SetItemAsync(key, obj, cacheTime: null);

    public async Task<TModel?> SetItemAsync(string key, TModel? obj, CacheDuration duration)
        => await SetItemAsync(key, obj, duration.ToTimeSpan());

    public async Task<TModel?> SetItemAsync(string key, TModel? obj, TimeSpan? cacheTime)
    {
        try
        {
            if (obj is null)
                return obj;

            await cacheBase.SetItemAsync(key, obj.Serialize(), cacheTime);
            return obj;
        }
        catch
        {
            return obj;
        }
    }

    public async Task<TModel?> SetItemIfAsync(bool condition, string key, TModel? obj)
        => condition ?
          await SetItemAsync(key, obj, cacheTime: null)
        : obj;

    public async Task<TModel?> SetItemIfAsync(bool condition, string key, TModel? obj, CacheDuration duration)
        => condition ?
          await SetItemAsync(key, obj, cacheTime: duration.ToTimeSpan())
        : obj;
}
