﻿namespace RedisCacheManager.Implementation;

public class PortoCache(ICacheBase cacheBase) : IPortoCache
{
    public async Task<TModel?> GetItemAsync<TModel>(string key) where TModel : IMessage<TModel>, new()
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

    public async Task<TModel?> GetOrderSetItemAsync<TModel>(string key, Func<Task<TModel>> func) where TModel : IMessage<TModel>, new()
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

    public async Task<TModel?> GetOrderSetItemAsync<TModel>(string key, CacheDuration cacheDuration, Func<Task<TModel>> func) where TModel : IMessage<TModel>, new()
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

    public async Task RemoveItemAsync(string key)
        => await cacheBase.RemoveItemAsync(key);

    public async Task<TModel?> SetItemAsync<TModel>(string key, TModel? obj) where TModel : IMessage<TModel>
        => await SetItemAsync(key, obj, cacheTime: null);

    public async Task<TModel?> SetItemAsync<TModel>(string key, TModel? obj, CacheDuration duration) where TModel : IMessage<TModel>
        => await SetItemAsync(key, obj, duration.ToTimeSpan());

    public async Task<TModel?> SetItemAsync<TModel>(string key, TModel? obj, TimeSpan? cacheTime) where TModel : IMessage<TModel>
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

    public async Task<TModel?> SetItemIfAsync<TModel>(bool condition, string key, TModel? obj) where TModel : IMessage<TModel>
        => condition ?
          await SetItemAsync(key, obj, cacheTime: null)
        : obj;

    public async Task<TModel?> SetItemIfAsync<TModel>(bool condition, string key, TModel? obj, CacheDuration duration) where TModel : IMessage<TModel>
        => condition ?
          await SetItemAsync(key, obj, cacheTime: duration.ToTimeSpan())
        : obj;
}
