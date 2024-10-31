namespace RedisCacheManager.Configuration;

public record CacheConfigs(string ConnectionString, int Instance, CacheType Type = CacheType.Json);

public class Configs
{
    public static CacheConfigs CacheConfigs { get; set; } = null!;
}

public enum CacheType
{
    Json,
    Proto
}