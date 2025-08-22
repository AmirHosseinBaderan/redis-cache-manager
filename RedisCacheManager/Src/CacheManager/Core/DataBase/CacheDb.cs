using CacheManager.Configuration;
using StackExchange.Redis;

namespace CacheManager.Core;

public class CacheDb(ICacheCore core) : ICacheDb
{
    public async ValueTask DisposeAsync()
    {
        GC.SuppressFinalize(this);
        await core.DisposeAsync();
    }

    public async Task<IDatabase?> GetDataBaseAsync()
        => await GetDataBaseAsync(Configs.CacheConfigs.ConnectionString, Configs.CacheConfigs.Instance);

    public async Task<IDatabase?> GetDataBaseAsync(string connectionString, int instance)
    {
        try
        {
            var connection = await core.ConnectAsync(connectionString);
            if (connection is null)
                return null;

            return connection.GetDatabase(instance);
        }
        catch
        {
            throw;
        }
    }
}
