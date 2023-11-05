using FlightGearApi.DTO;
using FlightGearApi.Enums;
using FlightGearApi.FlightGearCore;
using FlightGearApi.UtilityClasses;
using Microsoft.AspNetCore.Mvc;

namespace FlightGearApi.Controllers;

[Route("api/pre-launch")]
public class LaunchController : Controller
{
    
    [HttpPost("configure-io")]
    public async Task<IActionResult> ConfigureIo([FromServices] IoManager ioManager)
    {
        ioManager.AddProperty(IoType.Output, FlightProperties.Throttle);
        ioManager.AddProperty(IoType.Output, FlightProperties.AltitudeMeters);
        return Ok(ioManager.GenerateXmlOutputFileContent());
    }
        
    [HttpPost("add-connection")]
    public async Task<IActionResult> ConfigureLaunch([FromServices] FlightGearLauncher launchManager, [FromBody] GenericConnectionRequest connectionRequest)
    {
        // var result = launchManager.AddGenericConnectionParameter(connectionRequest);
        return Ok();
    }
        
    [HttpGet("config")]
    public async Task<IActionResult> GetLaunchConfig([FromServices] FlightGearLauncher launchManager)
    {
        return Ok();
    }
    
    [HttpPost("launch-simulation")]
    public async Task<IActionResult> LaunchSimulation([FromServices] FlightGearLauncher launcher)
    {
        launcher.LaunchSimulation();
        return Ok();
    }
    
    [HttpPost("exit-simulation")]
    public async Task<IActionResult> ExitSimulation([FromServices] FlightGearLauncher launcher)
    {
        launcher.ExitSimulation();
        return Ok();
    }
}