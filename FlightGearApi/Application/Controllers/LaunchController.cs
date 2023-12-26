using FlightGearApi.Application.DTO;
using FlightGearApi.Domain.Enums;
using FlightGearApi.Domain.FlightGearCore;
using FlightGearApi.Domain.Records;
using FlightGearApi.Infrastructure.Interfaces;
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
    /// Запустить симуляцию
    /// </summary>
    [HttpPost("start")]
    public async Task<IActionResult> LaunchSimulation([FromServices] IPostgresDatabase database,[FromBody] LaunchSessionRequestDto parameters)
    {
        if (_manipulator.Stages.Count < 1)
        {
            return BadRequest("No stages added");
        }
        if (await _launcher.TryLaunchSimulation(parameters.SessionName, parameters.RefreshesPerSecond, database))
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