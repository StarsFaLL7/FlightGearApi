using FlightGearApi.DTO;
using FlightGearApi.Enums;
using FlightGearApi.FlightGearCore;
using FlightGearApi.UtilityClasses;
using Microsoft.AspNetCore.Mvc;

namespace FlightGearApi.Controllers;

[Route("api/pre-launch")]
public class LaunchController : Controller
{
    private readonly IConfiguration _configuration;
    
    public LaunchController(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    [HttpPost("save-protocol-files")]
    [ProducesResponseType(typeof(FlightPropertiesResponse), 200)]
    public async Task<IActionResult> SaveXmlFiles([FromServices] IoManager ioManager)
    {
        var path = Path.Combine(_configuration.GetSection("FlightGear:Path").Value,
            _configuration.GetSection("FlightGear:ProtocolSubPath").Value);
        ioManager.SaveInputXmlFile(path);
        ioManager.SaveOutputXmlFile(path);
        
        return Ok(ioManager.GetAllIoParameters());
    }
    
    [HttpGet("get-input-properties")]
    public async Task<IActionResult> GetInputProperties([FromServices] IoManager ioManager)
    {
        var result = ioManager.GetAllIoParameters().InputProperties;
        return Ok(result);
    }
    
    [HttpGet("get-output-properties")]
    public async Task<IActionResult> GetOutputProperties([FromServices] IoManager ioManager)
    {
        var result = ioManager.GetAllIoParameters().OutputProperties;
        return Ok(result);
    }
    
    [HttpPost("add-flight-property")]
    public async Task<IActionResult> AddFlightProperty([FromServices] IoManager ioManager, [FromBody] FlightPropertyAddRequest dto)
    {
        if (ioManager.AddProperty(dto.IoType, dto.Path, dto.Name, dto.TypeName))
        {
            var result = ioManager.GetAllIoParameters();
            return Ok(result);
        }
        return BadRequest("This property is already in the list.");
    }
    
    [HttpPost("remove-flight-property")]
    public async Task<IActionResult> RemoveFlightProperty([FromServices] IoManager ioManager, [FromBody] FlightPropertyRemoveRequest dto)
    {
        if (ioManager.TryRemoveProperty(dto.IoType, dto.Name))
        {
            var result = ioManager.GetAllIoParameters();
            return Ok(result);
        }
        return BadRequest("There is no property with the given name in the list.");
    }
    
    [HttpPost("add-connection")]
    public async Task<IActionResult> AddGenericConnection([FromServices] FlightGearLauncher launchManager, [FromBody] GenericConnectionRequest connectionRequest)
    {
        // TODO
        // var result = launchManager.AddGenericConnectionParameter(connectionRequest);
        return Ok();
    }
        
    [HttpGet("config")]
    public async Task<IActionResult> GetLaunchConfig([FromServices] FlightGearLauncher launchManager)
    {
        // TODO
        return Ok();
    }
    
    [HttpPost("launch-simulation")]
    public async Task<IActionResult> LaunchSimulation([FromServices] FlightGearLauncher launcher)
    {
        // TODO
        launcher.LaunchSimulation();
        return Ok();
    }
    
    [HttpPost("exit-simulation")]
    public async Task<IActionResult> ExitSimulation([FromServices] FlightGearLauncher launcher)
    {
        // TODO
        launcher.ExitSimulation();
        return Ok();
    }
}