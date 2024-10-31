namespace RedisCacheManager.Abstraction;

public interface ICache<TModel>
{
    Task<TModel?> GetItemAsync(string key);

    Task<TModel?> SetItemAsync(string key, TModel? obj);

    Task<TModel?> SetItemAsync(string key, TModel? obj, CacheDuration duration);

    Task<TModel?> SetItemIfAsync(bool condition, string key, TModel? obj);

    Task<TModel?> SetItemIfAsync(bool condition, string key, TModel? obj, CacheDuration duration);

    Task RemoveItemAsync(string key);

    Task<TModel?> GetOrderSetItemAsync(string key, Func<Task<TModel>> action);

    Task<TModel?> GetOrderSetItemAsync(string key, Func<TModel> action);

    Task<TModel?> GetOrderSetItemAsync(string key, CacheDuration cacheDuration, Func<Task<TModel>> action);

    Task<TModel?> GetOrderSetItemAsync(string key, CacheDuration cacheDuration, Func<TModel> action);

    Task<TModel?> SetItemAsync(string key, TModel? obj, TimeSpan? cacheTime);
}
