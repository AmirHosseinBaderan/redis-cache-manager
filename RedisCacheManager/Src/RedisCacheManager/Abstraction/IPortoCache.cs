using Google.Protobuf;
using RedisCacheManager.Configuration;

namespace RedisCacheManager.Abstraction;

public interface IPortoCache : IAsyncDisposable
{
    Task<TModel?> GetItemAsync<TModel>(string key) where TModel : IMessage<TModel>;

    Task<TModel?> SetItemAsync<TModel>(string key, TModel? obj) where TModel : IMessage<TModel>;

    Task<TModel?> SetItemAsync<TModel>(string key, TModel? obj, CacheDuration duration) where TModel : IMessage<TModel>;

    Task<TModel?> SetItemIfAsync<TModel>(bool condition, string key, TModel? obj) where TModel : IMessage<TModel>;

    Task<TModel?> SetItemIfAsync<TModel>(bool condition, string key, TModel? obj, CacheDuration duration) where TModel : IMessage<TModel>;

    Task RemoveItemAsync(string key);

    Task<TModel?> GetOrderSetItemAsync<TModel>(string key, Func<TModel> action) where TModel : IMessage<TModel>;

    Task<TModel?> GetOrderSetItemAsync<TModel>(string key, CacheDuration cacheDuration, Func<Task<TModel>> action) where TModel : IMessage<TModel>;

    Task<TModel?> SetItemAsync<TModel>(string key, TModel obj, TimeSpan? cacheTime) where TModel : IMessage<TModel>;
}
