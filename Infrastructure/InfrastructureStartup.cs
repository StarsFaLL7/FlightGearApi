using Application.Interfaces.Connection;
using Infrastructure.FlightGearConnection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Infrastructure;

public static class InfrastructureStartup
{
    public static IServiceCollection TryAddInfrastructure(this IServiceCollection services)
    { 
        services.TryAddSingleton<IConnectionManager, ConnectionManager>();
        services.TryAddSingleton<IConnectionReader>(provider => provider.GetRequiredService<IConnectionManager>());
        services.TryAddSingleton<IConnectionSender>(provider => provider.GetRequiredService<IConnectionManager>());
        
        return services;
    }
}