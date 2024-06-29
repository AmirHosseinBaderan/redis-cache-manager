using Microsoft.Extensions.DependencyInjection;
using RedisCacheManager.Core;

namespace RedisCacheManager.Configuration;

public static class Configuration
{
    public static IServiceCollection AddRedisCacheManager(this IServiceCollection services, string connectionString)
    {
        // Set connection string to configs
        Configs.ConnectionString = connectionString;

        // Add cache core services
        services.AddScoped<ICacheCore, CacheCore>();



        return services;
    }
}
