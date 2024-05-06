using Application.Interfaces;
using Application.Interfaces.Entities;
using Application.Services;
using Application.Services.Entities;
using Application.Services.Master;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Application;

public static class ApplicationLayerStartup
{
    public static IServiceCollection TryAddApplicationLayer(this IServiceCollection services)
    {
        services.TryAddSingleton<IUserAnalyticsMasterService, UserAnalyticsMasterService>();
        services.TryAddSingleton<IUserSimulationMasterService, UserSimulationMasterService>();
        
        services.TryAddSingleton<IFlightPlanService, FlightPlanService>();
        services.TryAddSingleton<IRunwayService, RunwayService>();
        services.TryAddSingleton<IAirportService, AirportService>();
        services.TryAddSingleton<ISessionService, SessionService>();
        services.TryAddSingleton<IFlightFunctionService, FlightFunctionService>();
        
        services.TryAddSingleton<IFlightExportedParametersReader, FlightExportedParametersReader>();
        services.TryAddSingleton<IFlightGearLauncher, FlightGearLauncher>();
        services.TryAddSingleton<IFlightManipulator, FlightManipulator>();
        services.TryAddSingleton<IXmlFileManager, XmlFileManager>();
        
        return services;
    }
}