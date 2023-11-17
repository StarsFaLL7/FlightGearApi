using FlightGearApi.Application.DTO;
using FlightGearApi.Domain.Enums;
using FlightGearApi.Domain.FlightGearCore;
using FlightGearApi.Domain.Records;
using Microsoft.AspNetCore.Mvc;

namespace FlightGearApi.Application.Controllers;

/// <summary>
/// Контроллер настройки запуска полёта
/// </summary>
[Route("api/launch")]
public class LaunchController : Controller
{
    private readonly IConfiguration _configuration;
    
    public LaunchController(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    /// <summary>
    /// Сохранить все файлы XML (обязательно перед запуском)
    /// </summary>
    [HttpPost("save-protocol-files")]
    [ProducesResponseType(typeof(FlightPropertiesResponse), 200)]
    public async Task<IActionResult> SaveXmlFiles([FromServices] IoManager ioManager)
    {
        ioManager.SaveInputXmlFile();
        ioManager.SaveOutputXmlFile();
        
        return Ok(ioManager.GetAllIoParametersAsync());
    }
    
    [HttpGet("get-input-properties")]
    public async Task<IActionResult> GetInputProperties([FromServices] IoManager ioManager)
    {
        var result = await ioManager.GetAllIoParametersAsync();
        return Ok(result.InputProperties);
    }
    
    [HttpGet("get-output-properties")]
    public async Task<IActionResult> GetOutputProperties([FromServices] IoManager ioManager)
    {
        var result = await ioManager.GetAllIoParametersAsync();
        return Ok(result.OutputProperties);
    }
    
    [HttpPost("add-flight-property")]
    public async Task<IActionResult> AddFlightProperty([FromServices] IoManager ioManager, [FromBody] FlightPropertyAddRequest dto)
    {
        if (ioManager.AddProperty(dto.IoType, dto.Path, dto.Name, dto.TypeName))
        {
            var result = ioManager.GetAllIoParametersAsync();
            return Ok(result);
        }
        return BadRequest("This property is already in the list.");
    }
    
    [HttpPost("remove-flight-property")]
    public async Task<IActionResult> RemoveFlightProperty([FromServices] IoManager ioManager, [FromBody] FlightPropertyRemoveRequest dto)
    {
        if (ioManager.TryRemoveProperty(dto.IoType, dto.Name))
        {
            var result = await ioManager.GetAllIoParametersAsync();
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
    
    [HttpPost("start-listen-test")]
    public async Task<IActionResult> StartListen([FromServices] ConnectionListener listener, [FromBody] GenericConnectionRequest connectionRequest)
    {
        listener.StartListen(new GenericConnectionInfo(IoType.Output,6789,1,"ds"), "test1");
        
        return Ok(await listener.GetCurrentValuesAsync("test1"));
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