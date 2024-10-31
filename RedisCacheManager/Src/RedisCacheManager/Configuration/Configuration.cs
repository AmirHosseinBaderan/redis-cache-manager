using Microsoft.Extensions.DependencyInjection;
using RedisCacheManager.Implementation;

namespace RedisCacheManager.Configuration;

public static class Configuration
{
    public static IServiceCollection AddRedisCacheManager(this IServiceCollection services, Func<CacheConfigs> config)
    {
        Configs.CacheConfigs = config();

        // Add cache core services
        services.AddScoped<ICacheCore, CacheCore>();

        services.AddScoped<ICacheDb, CacheDb>();
        services.AddScoped<ICacheBase, CacheBase>();

        Type cahceImplementation = Configs.CacheConfigs.Type switch
        {
            CacheType.Json => typeof(JsonCache<>),
            CacheType.Proto => typeof(ProtoCache<>),
            _ => typeof(JsonCache<>),
        };
        services.AddScoped(typeof(ICache<>), cahceImplementation);

        return services;
    }
}
