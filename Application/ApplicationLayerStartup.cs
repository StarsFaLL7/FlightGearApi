using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class ApplicationLayerStartup
{
    public static IServiceCollection TryAddApplicationLayer(this IServiceCollection services)
    {
        return services;
    }
}