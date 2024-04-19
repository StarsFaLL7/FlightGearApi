using Application.Interfaces;
using Application.Interfaces.Connection;
using Application.Interfaces.Repositories;
using Domain.Entities;
using Domain.Enums.FlightUtilityProperty;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[Route("test")]
public class TestController : Controller
{
    private readonly IConnectionManager _connectionManager;
    private readonly IXmlFileManager _xmlFileManager;
    private readonly IFlightGearLauncher _flightGearLauncher;
    private readonly IFlightPlanRepository _flightPlanRepository;
    private readonly IRoutePointRepository _routePointRepository;

    public TestController(IConnectionManager connectionManager, IXmlFileManager xmlFileManager, 
        IFlightGearLauncher flightGearLauncher, IFlightPlanRepository flightPlanRepository, 
        IRoutePointRepository routePointRepository)
    {
        _connectionManager = connectionManager;
        _xmlFileManager = xmlFileManager;
        _flightGearLauncher = flightGearLauncher;
        _flightPlanRepository = flightPlanRepository;
        _routePointRepository = routePointRepository;
    }
    
    [HttpGet("connections/current-value")]
    [ProducesResponseType(typeof(FlightPropertiesShot), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCurrentValues()
    {
        var res = await _connectionManager.GetCurrentValuesAsync();
        return Ok(res);
    }
    
    [HttpGet("connections/position")]
    [ProducesResponseType(typeof(Dictionary<string, double>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetUtilityValues()
    {
        var res = await _connectionManager.GetCurrentUtilityValuesAsync(FlightUtilityProperty.Altitude,
            FlightUtilityProperty.Latitude, FlightUtilityProperty.Longitude);
        return Ok(res);
    }
    
    [HttpPost("plans/initialize")]
    public async Task<IActionResult> CreateRouteFile([FromQuery] Guid flightPlanId)
    {
        var plan = await _flightPlanRepository.GetAggregateByIdAsync(flightPlanId);
        await _xmlFileManager.CreateOrUpdateRouteManagerXmlFileAsync(plan);
        await _flightGearLauncher.InitializeWithFlightPlanAsync(plan);
        return Ok();
    }
    
    [HttpPost("plans/save")]
    public async Task<IActionResult> SaveFlightPlanToDb([FromBody] FlightPlan flightPlan)
    {
        await _flightPlanRepository.SaveAsync(flightPlan);
        foreach (var point in flightPlan.RoutePoints)
        {
            await _routePointRepository.SaveAsync(point);
        }
        return Ok();
    }
    
    [HttpGet("plans")]
    public async Task<IActionResult> GetFlightPlanById([FromQuery] Guid planId)
    {
        var plan = await _flightPlanRepository.GetAggregateByIdAsync(planId);
        return Ok(plan);
    }
    
    [HttpGet("plans/all")]
    public async Task<IActionResult> GetAllFlightPlans()
    {
        return Ok(await _flightPlanRepository.GetAll());
    }
    
    [HttpGet("launch-arguments")]
    public async Task<IActionResult> GetLaunchArguments([FromQuery] int readsPerSecond)
    {
        return Ok(_flightGearLauncher.GetLaunchString(readsPerSecond));
    }
    
    [HttpPost("launch-flight-gear")]
    public async Task<IActionResult> LaunchFlightGear([FromQuery] int readsPerSecond)
    {
        await _flightGearLauncher.TryLaunchSimulationAsync(readsPerSecond);
        return Ok(_flightGearLauncher.GetLaunchString(readsPerSecond));
    }
}