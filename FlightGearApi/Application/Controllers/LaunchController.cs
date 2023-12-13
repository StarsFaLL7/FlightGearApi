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
    private readonly IoManager _ioManager;
    private readonly FlightGearLauncher _launcher;
    private readonly ConnectionListener _listener;
    private readonly FlightGearManipulator _manipulator;
    
    public LaunchController(IConfiguration configuration, IoManager ioManager, FlightGearLauncher launcher, ConnectionListener listener, FlightGearManipulator manipulator)
    {
        _configuration = configuration;
        _ioManager = ioManager;
        _launcher = launcher;
        _listener = listener;
        _manipulator = manipulator;
    }
    
    /// <summary>
    /// Получить все свойства полёта, которые будут сохраняться
    /// </summary>
    [ProducesResponseType(typeof(List<string>), 200)]
    [HttpGet("flight-properties")]
    public async Task<IActionResult> GetOutputProperties()
    {
        var result = _ioManager.GetListenPropertiesNames();
        return Ok(result.OutputProperties);
    }
    
    /// <summary>
    /// Добавить параметры, которые будут сохраняться
    /// </summary>
    [ProducesResponseType(typeof(FlightPropertiesResponse), 200)]
    [HttpPost("flight-properties")]
    public async Task<IActionResult> AddFlightProperty([FromBody] FlightPropertyAddRequest[] propertiesList)
    {
        foreach (var property in propertiesList)
        {
            _ioManager.TryAddProperty(property.Path, property.Name, property.TypeName);
        }
        var result = _ioManager.GetListenPropertiesNames();
        return Ok(result);
    }
    
    /// <summary>
    /// Удалить параметр, который будет сохраняться
    /// </summary>
    [ProducesResponseType(typeof(FlightPropertiesResponse), 200)]
    [HttpDelete("flight-properties/{name}")]
    public async Task<IActionResult> RemoveFlightProperty([FromRoute] string name)
    {
        if (_ioManager.TryRemoveListenProperty(name))
        {
            var result = _ioManager.GetListenPropertiesNames();
            return Ok(result);
        }
        return BadRequest("There is no property with the given name in the list.");
    }
    
    /// <summary>
    /// Запустить симуляцию
    /// </summary>
    [HttpPost("start")]
    public async Task<IActionResult> LaunchSimulation([FromBody] LaunchSessionRequestDto parameters)
    {
        if (await _launcher.TryLaunchSimulation(parameters.SessionName, parameters.RefreshesPerSecond))
        {
            return Ok();
        }
        return Conflict("Simulation already started.");
    }
    
    /// <summary>
    /// Выйти из симуляции
    /// </summary>
    [HttpPost("exit")]
    public async Task<IActionResult> ExitSimulation()
    {
        if (_launcher.IsRunning)
        {
            _launcher.Exit();
            return Ok();
        }
        
        return Conflict("Simulation already exited.");
    }
    
    /// <summary>
    /// Добавить этап для полёта с заданными характеристиками.
    /// </summary>
    /// <param name="stage">Новый этап полёта с индексом вставки</param>
    /// <returns></returns>
    [HttpPost("stages")]
    public async Task<IActionResult> AddFlightStage([FromBody] FlightStageModel stage)
    {
        if (_launcher.IsRunning)
        {
            return Conflict("Simulation is in progress.");
        }
        _manipulator.AddStage(stage);
        return Ok();
    }
    
    /// <summary>
    /// Получить все заданные этапы полёта.
    /// </summary>
    /// <returns></returns>
    [HttpGet("stages")]
    public async Task<IActionResult> GetFlightStages()
    {
        return Ok(_manipulator.Stages);
    }
    
    /// <summary>
    /// Удалить этап полёта по указанному индексу.
    /// </summary>
    /// <param name="index">Индекс, с которого удалить этап полёта.</param>
    /// <returns></returns>
    [HttpDelete("stages/{index:int}")]
    public async Task<IActionResult> RemoveFlightStages([FromRoute] int index)
    {
        if (index > _manipulator.Stages.Count - 1 || index < 0)
        {
            return BadRequest("Index is invalid");
        }

        var stage = _manipulator.Stages[index];
        _manipulator.Stages.RemoveAt(index);
        return Ok(stage);
    }
}