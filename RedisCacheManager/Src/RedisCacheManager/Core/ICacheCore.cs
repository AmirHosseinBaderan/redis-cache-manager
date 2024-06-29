using StackExchange.Redis;

namespace RedisCacheManager.Core;

internal interface ICacheCore : IAsyncDisposable
{
    /// <summary>
    /// Create connection with redis server use default configuration
    /// </summary>
    /// <returns><see cref="ConnectionMultiplexer"/> Instance </returns>
    Task<ConnectionMultiplexer?> ConnectAsync();

    /// <summary>
    /// Create connection with redis server use connection string
    /// </summary>
    /// <param name="connectionString">Connection string for connect to redis</param>
    /// <returns><see cref="ConnectionMultiplexer"/> Instance </returns>
    Task<ConnectionMultiplexer?> ConnectAsync(string connectionString);
}
