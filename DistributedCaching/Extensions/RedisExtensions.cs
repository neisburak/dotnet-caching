using DistributedCaching.Services;

namespace DistributedCaching.Extensions;

public static class RedisExtensions
{
    public static void UseRedis(this IHost app)
    {
        var redisService = app.Services.GetRequiredService<RedisService>();
        redisService.Connect();
    }
}