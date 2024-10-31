using Microsoft.Extensions.DependencyInjection;
using RedisCacheManager.Core.Factory;
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

        services.AddKeyedScoped(typeof(ICache<>), nameof(CacheType.Json), typeof(JsonCache<>));
        services.AddKeyedScoped(typeof(ICache<>), nameof(CacheType.Proto), typeof(ProtoCache<>));

        services.AddSingleton<ICacheFactory>(provider => new CacheFactory(Configs.CacheConfigs.Type, provider));

        services.AddScoped(provider =>
        {
            var cacheFactory = provider.GetRequiredService<ICacheFactory>();

            return Configs.CacheConfigs.Type switch
            {
                CacheType.Json => provider.GetRequiredKeyedService(typeof(ICache<>), nameof(CacheType.Json)),
                CacheType.Proto => provider.GetRequiredKeyedService(typeof(ICache<>), nameof(CacheType.Proto)),
                _ => provider.GetRequiredKeyedService(typeof(ICache<>), nameof(CacheType.Json)),
            };
        });


        return services;
    }
}
