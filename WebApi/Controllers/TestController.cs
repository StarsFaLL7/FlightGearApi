using Application.Interfaces.Connection;
using Domain.Entities;
using Domain.Enums.FlightUtilityProperty;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[Route("test")]
public class TestController : Controller
{
    private readonly IConnectionManager _connectionManager;

    public TestController(IConnectionManager connectionManager)
    {
        _connectionManager = connectionManager;
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
}