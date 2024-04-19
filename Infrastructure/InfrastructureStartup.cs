using Application.Interfaces.Connection;
using Application.Interfaces.Repositories;
using Infrastructure.FlightGearConnection;
using Infrastructure.Services.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Infrastructure;

public static class InfrastructureStartup
{
    public static IServiceCollection TryAddInfrastructure(this IServiceCollection services)
    {
        services.AddDbContext<PostgresDbContext>(contextLifetime: ServiceLifetime.Transient, optionsLifetime: ServiceLifetime.Singleton);
        services.TryAddSingleton<IConnectionManager, ConnectionManager>();
        services.TryAddSingleton<IConnectionReader>(provider => provider.GetRequiredService<IConnectionManager>());
        services.TryAddSingleton<IConnectionSender>(provider => provider.GetRequiredService<IConnectionManager>());
        
        services.TryAddTransient<IAirportRepository, AirportRepository>();
        services.TryAddTransient<IAirportRunwayRepository, AirportRunwayRepository>();
        services.TryAddTransient<IFlightPlanRepository, FlightPlanRepository>();
        services.TryAddTransient<IFlightPropertiesShotRepository, FlightPropertyShotRepository>();
        services.TryAddTransient<IFlightSavedSessionsRepository, FlightSavedSessionsRepository>();
        services.TryAddTransient<IFunctionPointRepository, FunctionPointRepository>();
        services.TryAddTransient<IReadyFlightFunctionRepository, ReadyFlightFunctionRepository>();
        services.TryAddTransient<IRoutePointRepository, RoutePointRepository>();
        return services;
    }
}