using Microsoft.Extensions.DependencyInjection;
using Shared.Models;
using Shared.Services;
using Shared.Services.Interfaces;
using Shared.Utilities;

namespace Shared.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection serviceCollection, Action<RedisOptions>? options = null)
    {
        // StackExchange.Redis API Service
        serviceCollection.AddSingleton<RedisService>(factory =>
        {
            var redisOptions = new RedisOptions { Configuration = "localhost:6379" };
            if (options is not null) options(redisOptions);

            return new RedisService(redisOptions.Configuration);
        });

        serviceCollection.AddScoped<IPostService, PostManager>();
        serviceCollection.AddScoped<IUserService>(factory =>
        {
            var redisService = factory.GetRequiredService<RedisService>();
            return new UserManagerWithCacheDecorator(new UserManager(), redisService);
        });

        return serviceCollection;
    }
}