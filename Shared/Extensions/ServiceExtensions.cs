using Microsoft.Extensions.DependencyInjection;
using Shared.Services;
using Shared.Services.Interfaces;

namespace Shared.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection serviceCollection)
    {
        return serviceCollection.AddScoped<IPostService, PostManager>();
    }
}