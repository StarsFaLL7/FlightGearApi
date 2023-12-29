using FlightGearApi.Application.DTO;
using FlightGearApi.Domain.Enums;
using FlightGearApi.Domain.FlightGearCore;
using FlightGearApi.Domain.Records;
using FlightGearApi.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FlightGearApi.Application.Controllers;

/// <summary>
/// Контроллер для ПМ Планирования
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
    /// Запустить симуляцию с ранее сохраннёными этапами полёта
    /// </summary>
    /// <param name="parameters">Модель с указанием названия сессии (sessionName) и кол-вом считываний параметров в секунду (refreshesPerSecond)</param>
    /// <returns></returns>
    [HttpPost("start")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string),StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string),StatusCodes.Status409Conflict)]
    public async Task<IActionResult> LaunchSimulation([FromServices] IPostgresDatabase database,[FromBody] LaunchSessionRequestDto parameters)
    {
        if (_manipulator.Stages.Count < 1)
        {
            return BadRequest("No flight stages have been added.");
        }
        if (await _launcher.TryLaunchSimulation(parameters.SessionName, parameters.RefreshesPerSecond, database))
        {
            return Ok();
        }
        return Conflict("The simulator is currently running.");
    }
    
    /// <summary>
    /// Ручной выход из симуляции (использовать только в крит. случаях)
    /// </summary>
    [HttpPost("exit")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string),StatusCodes.Status409Conflict)]
    public async Task<IActionResult> ExitSimulation()
    {
        if (_launcher.IsRunning)
        {
            _launcher.Exit();
            return Ok();
        }
        
        return Conflict("The simulator is not currently running..");
    }
    
    /// <summary>
    /// Добавить этап для полёта с заданными характеристиками
    /// </summary>
    /// <param name="stage">Новый этап полёта с индексом вставки</param>
    [HttpPost("stages")]
    [ProducesResponseType(typeof(FlightStageDto),StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string),StatusCodes.Status409Conflict)]
    public async Task<IActionResult> AddFlightStage([FromBody] FlightStageDto stage)
    {
        if (_launcher.IsRunning)
        {
            return Conflict("The simulator is currently running, so stages cannot be edited.");
        }
        _manipulator.AddStage(stage);
        return Ok(stage);
    }
    
    /// <summary>
    /// Получить все сохранённые этапы полёта.
    /// </summary>
    /// <returns></returns>
    [HttpGet("stages")]
    [ProducesResponseType(typeof(List<FlightStageDto>),StatusCodes.Status200OK)]
    public async Task<IActionResult> GetFlightStages()
    {
        return Ok(_manipulator.Stages);
    }
    
    /// <summary>
    /// Удалить этап полёта по указанному индексу.
    /// </summary>
    /// <param name="index">Индекс этапа полёта, который нужно удалить.</param>
    /// <returns></returns>
    [HttpDelete("stages/{index:int}")]
    [ProducesResponseType(typeof(FlightStageDto),StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string),StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(string),StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RemoveFlightStage([FromRoute] int index)
    {
        if (index > _manipulator.Stages.Count - 1 || index < 0)
        {
            if (_manipulator.Stages.Count == 0)
            {
                return BadRequest($"Index is invalid. Stages sequence has 0 items.");
            }
            return BadRequest($"Index is invalid. Index must be between 0 and {_manipulator.Stages.Count-1}");
        }
        if (_launcher.IsRunning)
        {
            return Conflict("The simulator is currently running, so stages cannot be edited.");
        }
        var removedStage = _manipulator.RemoveStage(index);
        return Ok(removedStage);
    }
    
    /// <summary>
    /// Изменить параметры этапа полёта на указанном индексе.
    /// </summary>
    /// <param name="index">Индекс этапа полёта, который нужно обновить.</param>
    /// <param name="updatedStage">Новые значения параметров этапа полёта.</param>
    /// <returns></returns>
    [HttpPut("stages/{index:int}")]
    [ProducesResponseType(typeof(FlightStageDto),StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string),StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(string),StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> EditFlightStage([FromRoute] int index, [FromBody] FlightStageDto updatedStage)
    {
        if (index > _manipulator.Stages.Count - 1 || index < 0)
        {
            if (_manipulator.Stages.Count == 0)
            {
                return BadRequest($"Index is invalid. Stages sequence has 0 items.");
            }
            return BadRequest($"Index is invalid. Index must be between 0 and {_manipulator.Stages.Count-1}");
        }
        if (_launcher.IsRunning)
        {
            return Conflict("The simulator is currently running, so stages cannot be edited.");
        }

        _manipulator.UpdateStage(updatedStage, index);
        return Ok(index);
    }
}