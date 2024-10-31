using Microsoft.Extensions.DependencyInjection;
using RedisCacheManager.Implementation;

namespace RedisCacheManager.Core.Factory;

public class CacheFactory : ICacheFactory
{
    private readonly CacheType _cacheType;
    private readonly IServiceProvider _serviceProvider;

    public CacheFactory(CacheType cacheType, IServiceProvider serviceProvider)
    {
        _cacheType = cacheType;
        _serviceProvider = serviceProvider;
    }

    public ICache<TModel> CreateJsonCache<TModel>()
        => _serviceProvider.GetRequiredService<JsonCache<TModel>>();

    public ICache<TModel> CreateProtoCache<TModel>() where TModel : IMessage<TModel>, new()
        => _serviceProvider.GetRequiredService<ProtoCache<TModel>>();
}