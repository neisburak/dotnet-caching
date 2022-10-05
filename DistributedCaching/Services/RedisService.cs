using StackExchange.Redis;

namespace DistributedCaching.Services;

public class RedisService
{
    private readonly string _configuration;
    private ConnectionMultiplexer _redis = default!;

    public RedisService(IConfiguration configuration)
    {
        _configuration = configuration["Redis:Configuration"];
    }

    public void Connect() => _redis = ConnectionMultiplexer.Connect(_configuration);
    public IDatabase GetDatabase(int db = 0) => _redis.GetDatabase(db);
}