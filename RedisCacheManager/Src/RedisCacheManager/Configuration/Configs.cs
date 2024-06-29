namespace RedisCacheManager.Configuration;

public record CacheConfigs(string ConnectionString, int Instance);

internal class Configs
{
    public static CacheConfigs CacheConfigs { get; set; } = null!;
}