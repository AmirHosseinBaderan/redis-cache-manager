namespace RedisCacheManager.Configuration;

public record CacheConfigs(string ConnectionString, int Instance, JsonSerializerSettings? JsonSerializerSettings, Formatting Formatting = Formatting.None);

public class Configs
{
    public static CacheConfigs CacheConfigs { get; set; } = null!;
}