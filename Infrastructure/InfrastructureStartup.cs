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
        
        services.TryAddScoped<IAirportRepository, AirportRepository>();
        services.TryAddScoped<IAirportRunwayRepository, AirportRunwayRepository>();
        services.TryAddScoped<IFlightPlanRepository, FlightPlanRepository>();
        services.TryAddScoped<IFlightPropertiesShotRepository, FlightPropertyShotRepository>();
        services.TryAddScoped<IFlightSavedSessionsRepository, FlightSavedSessionsRepository>();
        services.TryAddScoped<IFunctionPointRepository, FunctionPointRepository>();
        services.TryAddScoped<IReadyFlightFunctionRepository, ReadyFlightFunctionRepository>();
        services.TryAddScoped<IRoutePointRepository, RoutePointRepository>();
        return services;
    }
}