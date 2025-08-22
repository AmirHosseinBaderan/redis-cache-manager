using CacheManager.Abstraction;
using CacheManager.Core;
using CacheManager.Implementation;
using Microsoft.Extensions.DependencyInjection;

namespace CacheManager.Configuration;

public static class Configuration
{
    public static IServiceCollection AddRedisCacheManager(this IServiceCollection services, Func<CacheConfigs> config)
    {
        Configs.CacheConfigs = config();

        // Add cache core services
        services.AddScoped<ICacheCore, CacheCore>();

        services.AddScoped<ICacheDb, CacheDb>();
        services.AddScoped<ICacheBase, CacheBase>();

        services.AddScoped<IJsonCache, JsonCache>();
        services.AddScoped<IProtoCache, ProtoCache>();

        return services;
    }
}
