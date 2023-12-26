using FlightGearApi.Application.DTO;
using FlightGearApi.Domain.Enums;
using FlightGearApi.Domain.FlightGearCore;
using FlightGearApi.Domain.Records;
using FlightGearApi.Domain.UtilityClasses;
using FlightGearApi.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FlightGearApi.Application.Controllers;

[Route("api/test")]
public class TestController : Controller
{
    private readonly IConfiguration _configuration;
    private readonly IoManager _ioManager;
    private readonly FlightGearLauncher _launcher;
    private readonly ConnectionListener _listener;
    private readonly FlightGearManipulator _manipulator;
    private readonly ExportParametersManager _exportManager;
    private readonly IPostgresDatabase _database;
    
    public TestController(IConfiguration configuration, IoManager ioManager, FlightGearLauncher launcher, 
        ConnectionListener listener, FlightGearManipulator manipulator, ExportParametersManager exportManager,
        IPostgresDatabase database)
    {
        _configuration = configuration;
        _ioManager = ioManager;
        _launcher = launcher;
        _listener = listener;
        _manipulator = manipulator;
        _exportManager = exportManager;
        _database = database;
    }
    
    [HttpGet("get-launch-string")]
    public async Task<IActionResult> GetLaunchString()
    {
        var result = _launcher.GenerateLaunchArguments();
        
        return Ok(result);
    }
    
    [HttpGet("current-properties")]
    public async Task<IActionResult> GetCurrentProperties()
    {
        var result = await _listener.GetCurrentValuesTelnetAsync();
        
        return Ok(result);
    }
    
    [HttpGet("current-utility-properties")]
    public async Task<IActionResult> GetCurrentUtilityProperties()
    {
        var result = await _listener.GetCurrentValuesTelnetAsync(true);
        
        return Ok(result);
    }
    
    [HttpPost("xml-file")]
    public async Task<IActionResult> GetXmlFileContent([FromBody] IoType type)
    {
        var result = _ioManager.GenerateXmlInputFileContent();
        return Ok(result);
    }
    
    [HttpPost("set-parameter")]
    public async Task<IActionResult> SetParameterTest()
    {
        await _manipulator.SendParametersAsync(new Dictionary<UtilityProperty, double>()
        {
            { UtilityProperty.ParkingBrake, 0},
            { UtilityProperty.Aileron, 0.1}
        });
        return Ok();
    }
    
    [HttpPost("fly-forward")]
    public async Task<IActionResult> FlyForwardTest()
    {
        _manipulator.ShouldFlyForward = true;
        _manipulator.FlyCycle();
        return Ok();
    }
    
    [HttpPost("export-to-bd")]
    public async Task<IActionResult> ExportToBd()
    {
        var properties = _exportManager.GetExportedProperties(10);
        _database.CreatePropertiesFromRange(properties, 10);
        return Ok();
    }
}