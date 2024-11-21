using Microsoft.Extensions.Logging;

namespace RedisCacheManager.Core;

public class CacheCore(ILogger<CacheCore> logger) : ICacheCore
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
            logger.LogInformation("Success connection with redis");
            return _connection;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Exception in redis connection");
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
