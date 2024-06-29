using RedisCacheManager.Configuration;
using StackExchange.Redis;

namespace RedisCacheManager.Core;

internal class CacheDb(ICacheCore core) : ICacheDb
{
    public async Task<IDatabase?> GetDataBaseAsync()
        => await GetDataBaseAsync(Configs.CacheConfigs.ConnectionString);

    public async Task<IDatabase?> GetDataBaseAsync(string connectionString)
    {
        try
        {
            var connection = await core.ConnectAsync(connectionString);
            if (connection is null)
                return null;

            return connection.GetDatabase(Configs.CacheConfigs.Instance);
        }
        catch
        {
            throw;
        }
    }
}
