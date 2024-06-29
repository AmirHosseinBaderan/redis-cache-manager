using RedisCacheManager.Configuration;
using StackExchange.Redis;

namespace RedisCacheManager.Core;

public class CacheCore : ICacheCore
{
    private ConnectionMultiplexer? _connection;

    public async Task<ConnectionMultiplexer?> ConnectAsync()
        => await ConnectAsync(Configs.CacheConfigs.ConnectionString);

    public async Task<ConnectionMultiplexer?> ConnectAsync(string connectionString)
    {
        try
        {
            if (_connection is not null and { IsConnected: true })
                return _connection;

            _connection = await ConnectionMultiplexer.ConnectAsync(connectionString);
            return _connection;
        }
        catch
        {
            throw;
        }
    }

    public async ValueTask DisposeAsync()
    {
        GC.SuppressFinalize(this);
        if (_connection is not null)
            await _connection.DisposeAsync();
    }
}
