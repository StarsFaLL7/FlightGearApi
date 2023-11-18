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
    /// Задать новое значение для кол-ва обновления данных в секунду.
    /// </summary>
    /// <param name="count">Новое значение</param>
    [ProducesResponseType(typeof(double), 200)]
    [ProducesResponseType(400)]
    [HttpPost("flight-properties/refreshes/{count:double}")]
    public async Task<IActionResult> SetRefreshesPerSecond([FromServices] IoManager ioManager, [FromRoute] double count)
    {
        if (ioManager.TrySetRefreshesPerSecond(count))
        {
            return Ok(ioManager.ConnectionRefreshesPerSecond);
        }

        return BadRequest("Invalid value for refreshes count.");
    }
    
    /// <summary>
    /// Получить значение кол-ва обновлений данных в секунду
    /// </summary>
    [ProducesResponseType(typeof(double), 200)]
    [HttpGet("flight-properties/refreshes")]
    public async Task<IActionResult> GetRefreshesPerSecond([FromServices] IoManager ioManager)
    {
        return Ok(ioManager.ConnectionRefreshesPerSecond);
    }
    
    /// <summary>
    /// Получить все свойства полёта, которые будут сохраняться
    /// </summary>
    [ProducesResponseType(typeof(List<string>), 200)]
    [HttpGet("flight-properties")]
    public async Task<IActionResult> GetOutputProperties([FromServices] IoManager ioManager)
    {
        var result = ioManager.GetAllIoPropertiesAsync();
        return Ok(result.OutputProperties);
    }
    
    /// <summary>
    /// Добавить параметры, которые будут сохраняться
    /// </summary>
    [ProducesResponseType(typeof(FlightPropertiesResponse), 200)]
    [HttpPost("flight-properties")]
    public async Task<IActionResult> AddFlightProperty([FromServices] IoManager ioManager, [FromBody] FlightPropertyAddRequest[] propertiesList)
    {
        foreach (var property in propertiesList)
        {
            ioManager.TryAddProperty(IoType.Output, property.Path, property.Name, property.TypeName);
        }
        var result = ioManager.GetAllIoPropertiesAsync();
        return Ok(result);
    }
    
    /// <summary>
    /// Удалить параметр, который будет сохраняться
    /// </summary>
    [ProducesResponseType(typeof(FlightPropertiesResponse), 200)]
    [HttpDelete("flight-properties/{name}")]
    public async Task<IActionResult> RemoveFlightProperty([FromServices] IoManager ioManager, [FromRoute] string name)
    {
        if (ioManager.TryRemoveProperty(IoType.Output, name))
        {
            var result = ioManager.GetAllIoPropertiesAsync();
            return Ok(result);
        }
        return BadRequest("There is no property with the given name in the list.");
    }
    
    /// <summary>
    /// Запустить симуляцию
    /// </summary>
    [HttpPost("start")]
    public async Task<IActionResult> LaunchSimulation([FromServices] FlightGearLauncher launcher)
    {
        if (await launcher.TryLaunchSimulation("test"))
        {
            return Ok();
        }
        return BadRequest("Simulation already started.");
    }
    
    /// <summary>
    /// Выйти из симуляции
    /// </summary>
    [HttpPost("exit")]
    public async Task<IActionResult> ExitSimulation([FromServices] FlightGearLauncher launcher)
    {
        if (launcher.TryExitSimulation())
        {
            return Ok();
        }

        return BadRequest("Simulation already exited.");
    }
}