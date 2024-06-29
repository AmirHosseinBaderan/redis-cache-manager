using Microsoft.Extensions.DependencyInjection;
using RedisCacheManager.Core;

namespace RedisCacheManager.Configuration;

public static class Configuration
{
    public static IServiceCollection AddRedisCacheManager(this IServiceCollection services, Func<CacheConfigs> config)
    {
        Configs.CacheConfigs = config();

        // Add cache core services
        services.AddScoped<ICacheCore, CacheCore>();



        return services;
    }
}
