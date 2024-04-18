using Application.Interfaces;
using Application.Interfaces.Connection;
using Domain.Entities;
using Domain.Enums.FlightUtilityProperty;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[Route("test")]
public class TestController : Controller
{
    private readonly IConnectionManager _connectionManager;
    private readonly IXmlFileManager _xmlFileManager;

    public TestController(IConnectionManager connectionManager, IXmlFileManager xmlFileManager)
    {
        _connectionManager = connectionManager;
        _xmlFileManager = xmlFileManager;
    }
    
    [HttpGet("get-current-values")]
    [ProducesResponseType(typeof(FlightPropertiesShot), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCurrentValues()
    {
        var res = await _connectionManager.GetCurrentValuesAsync();
        return Ok(res);
    }
    
    [HttpGet("get-utility-values")]
    [ProducesResponseType(typeof(Dictionary<string, double>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetUtilityValues()
    {
        var res = await _connectionManager.GetCurrentUtilityValuesAsync(FlightUtilityProperty.Altitude,
            FlightUtilityProperty.Latitude, FlightUtilityProperty.Longitude);
        return Ok(res);
    }
    
    [HttpPost("updateRouteFile")]
    public async Task<IActionResult> CreateRouteFile([FromBody] FlightPlan flightPlan)
    {
        await _xmlFileManager.CreateOrUpdateRouteManagerXmlFileAsync(flightPlan);
        return Ok();
    }
    
    [HttpGet("getTestJsonRoute")]
    public async Task<IActionResult> GetTestRoute()
    {
        var plan = new FlightPlan
        {
            Title = "Test",
            Remarks = "Тестовый маршрут над Питером без взлета и посадки.",
            RoutePoints = new List<Domain.Entities.RoutePoint>(),
            Id = Guid.NewGuid()
        };
        var point1 = new Domain.Entities.RoutePoint
        {
            Order = 0,
            Longitude = 30.23026,
            Latitude = 59.77368,
            Altitude = 1000,
            FlightPlanId = plan.Id,
            Id = Guid.NewGuid()
        };
        var point2 = new Domain.Entities.RoutePoint
        {
            Order = 1,
            Longitude = 30.27557,
            Latitude = 59.83101,
            Altitude = 1200,
            FlightPlanId = plan.Id,
            Id = Guid.NewGuid()
        };
        var point3 = new Domain.Entities.RoutePoint
        {
            Order = 2,
            Longitude = 30.12451,
            Latitude = 59.82239,
            Altitude = 900,
            FlightPlanId = plan.Id,
            Id = Guid.NewGuid()
        };
        plan.RoutePoints.Add(point1);
        plan.RoutePoints.Add(point2);
        plan.RoutePoints.Add(point3);
        return Ok(plan);
    }
}