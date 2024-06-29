namespace RedisCacheManager.Configuration;

public record CacheConfigs(string ConnectionString, int Instance);

public class Configs
{
    public static CacheConfigs CacheConfigs { get; set; } = null!;
}