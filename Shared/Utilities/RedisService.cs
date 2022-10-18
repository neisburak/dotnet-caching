using StackExchange.Redis;

namespace Shared.Utilities;

public class RedisService
{
    private ConnectionMultiplexer _connectionMultiplexer = default!;

    public RedisService(string configuration)
    {
        _connectionMultiplexer = ConnectionMultiplexer.Connect(configuration);
    }

    public IDatabase GetDatabase(int db = 0) => _connectionMultiplexer.GetDatabase(db);
}