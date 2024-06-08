using Application.Enums;
using Application.Interfaces;
using Application.Interfaces.Connection;
using Application.Interfaces.Entities;
using Application.Interfaces.Repositories;
using Domain.Entities;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Services.Master;

public static class UserSimulationMasterService
{
    public static FlightStatus CurrentFlightStatus { get; private set; } = FlightStatus.NotRunning;
    public static FlightSession? CurrentRunningSession { get; private set; }
    
    public static async Task StartSimulationWithFlightPlanAsync(Guid flightPlanId, FlightSession flightSession, 
        IFlightPlanService flightPlanService, IXmlFileManager xmlFileManager, IFlightGearLauncher flightGearLauncher, IFlightManipulator flightManipulator)
    {
        CurrentFlightStatus = FlightStatus.Launching;
        try
        {
            var flightPlan = await flightPlanService.GetAggregatedFlightPlanAsync(flightPlanId);
            await xmlFileManager.CreateOrUpdateRouteManagerXmlFileAsync(flightPlan);
            await xmlFileManager.CreateOrUpdateExportXmlFileAsync();
            await flightGearLauncher.InitializeWithFlightPlanAsync(flightPlan);
            await flightGearLauncher.TryLaunchSimulationAsync(flightSession);
            SetFlightStatus(FlightStatus.Running);
            SetCurrentRunningSession(flightSession);
            flightManipulator.FlyCycleAsync(flightPlan);
        }
        catch (Exception e)
        {
            SetFlightStatus(FlightStatus.Exited);
            SetCurrentRunningSession(null);
            throw;
        }
    }
    
    public static void SetFlightStatus(FlightStatus newStatus)
    {
        CurrentFlightStatus = newStatus;
    }

    public static void SetCurrentRunningSession(FlightSession? session)
    {
        CurrentRunningSession = session;
    }
}