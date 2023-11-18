using FlightGearApi.Application.DTO;
using FlightGearApi.Domain.Enums;
using FlightGearApi.Domain.FlightGearCore;
using FlightGearApi.Domain.Records;
using Microsoft.AspNetCore.Mvc;

namespace FlightGearApi.Application.Controllers;

[Route("api/test")]
public class TestController : Controller
{
    private readonly IConfiguration _configuration;
    private readonly IoManager _ioManager;
    private readonly FlightGearLauncher _launcher;
    private readonly ConnectionListener _listener;
    
    public TestController(IConfiguration configuration, IoManager ioManager, FlightGearLauncher launcher, ConnectionListener listener)
    {
        _configuration = configuration;
        _ioManager = ioManager;
        _launcher = launcher;
        _listener = listener;
    }
    
    [HttpGet("get-launch-string")]
    public async Task<IActionResult> GetLaunchString()
    {
        var result = _launcher.GenerateAllParametersString();
        
        return Ok(result);
    }
    
    [HttpGet("current-properties")]
    public async Task<IActionResult> GetCurrentProperties()
    {
        var result = await _listener.GetCurrentValuesAsync("test1");
        
        return Ok(result);
    }
    
    [HttpPost("xml-file")]
    public async Task<IActionResult> GetXmlFileContent([FromBody] IoType type)
    {
        var result = type == IoType.Input
            ? _ioManager.GenerateXmlInputFileContent()
            : _ioManager.GenerateXmlOutputFileContent();
        
        return Ok(result);
    }
    
    [HttpPost("set-connection-refreshes-per-second")]
    public async Task<IActionResult> SetConnectionRefreshes([FromBody] int refreshes)
    {
        if (_ioManager.TrySetRefreshesPerSecond(refreshes))
        {
            return Ok();
        }

        return BadRequest("Invalid value for refreshes count.");
    }
}