﻿using Microsoft.Extensions.DependencyInjection;
using RedisCacheManager.Abstraction;
using RedisCacheManager.Core;
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

        services.AddScoped<ICache, Cache>();

        return services;
    }
}
