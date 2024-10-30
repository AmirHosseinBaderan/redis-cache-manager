namespace RedisCacheManager.Abstraction;

public interface ICache
{
    Task<TModel?> GetItemAsync<TModel>(string key);

    Task<TModel?> SetItemAsync<TModel>(string key, TModel? obj);

    Task<TModel?> SetItemAsync<TModel>(string key, TModel? obj, CacheDuration duration);

    Task<TModel?> SetItemIfAsync<TModel>(bool condition, string key, TModel? obj);

    Task<TModel?> SetItemIfAsync<TModel>(bool condition, string key, TModel? obj, CacheDuration duration);

    Task RemoveItemAsync(string key);

    Task<TModel?> GetOrderSetItemAsync<TModel>(string key, Func<TModel> action);

    Task<TModel?> GetOrderSetItemAsync<TModel>(string key, CacheDuration cacheDuration, Func<Task<TModel>> action);

    Task<TModel?> SetItemAsync<TModel>(string key, TModel obj, TimeSpan? cacheTime);
}
