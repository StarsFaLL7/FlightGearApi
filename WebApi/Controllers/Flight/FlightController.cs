using Application.Enums;
using Application.Interfaces;
using Application.Interfaces.Entities;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using WebApi.Controllers.Base;
using webapi.Controllers.Flight.Requests;
using webapi.Controllers.Flight.Responses;

namespace webapi.Controllers.Flight;

[Route("api/flight")]
public class FlightController : Controller
{
    private readonly IUserSimulationMasterService _masterService;
    private readonly ISessionService _sessionService;

    public FlightController(IUserSimulationMasterService masterService, ISessionService sessionService)
    {
        _masterService = masterService;
        _sessionService = sessionService;
    }
    
    /// <summary>
    /// Запуск симуляции полета
    /// </summary>
    [HttpPost("launch")]
    [ProducesResponseType(typeof(BasicStatusResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BasicStatusResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> LaunchSimulation([FromBody] LaunchSimulationRequest dto)
    {
        var flightSession = new FlightSession
        {
            Title = dto.Title,
            PropertiesReadsPerSec = dto.ReadsPerSecond,
            DateTimeStart = DateTime.Now.ToUniversalTime(),
            DurationSec = 0,
            Id = Guid.NewGuid()
        };
        await _sessionService.SaveSessionAsync(flightSession);
        try
        {
            await _masterService.StartSimulationWithFlightPlanAsync(dto.FlightPlanId, flightSession);
        }
        catch (Exception e)
        {
            await _sessionService.RemoveSessionAsync(flightSession.Id);
            throw;
        }
        
        return Ok(new BasicStatusResponse
        {
            Status = BasicStatusEnum.Success.ToString(),
            Comment = "Симуляция успешно запущена."
        });
    }
    
    /// <summary>
    /// Получение статуса полета
    /// </summary>
    [HttpGet("status")]
    [ProducesResponseType(typeof(FlightStatusResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BasicStatusResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetFlightStatus()
    {
        var status = await _masterService.GetSimulationStatus();
        var percentCompleted = await _masterService.GetRoutePercentCompletionAsync();
        var lastReachedWp = await _masterService.GetLastReachedRoutePointOrderAsync();
        var res = new FlightStatusResponse
        {
            Status = status.ToString(),
            IsRunning = status == FlightStatus.Running,
            LastReachedPointOrder = lastReachedWp,
            PercentCompleted = percentCompleted
        };
        return Ok(res);
    }
    
    /// <summary>
    /// Получение текущей геолокации самолета и некоторых параметров
    /// </summary>
    [HttpGet("properties")]
    [ProducesResponseType(typeof(FlightPositionResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BasicStatusResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetFlightProperties()
    {
        var status = await _masterService.GetSimulationStatus();
        var res = new FlightPositionResponse
        {
            Status = status.ToString(),
            Longitude = 0,
            Latitude = 0,
            AltitudeAgl = 0,
            Altitude = 0,
            Roll = 0,
            Pitch = 0,
            Heading = 0,
            IndicatedSpeed = 0,
            Airspeed = 0,
        };
        if (status != FlightStatus.Running)
        {
            return Ok(res);
        }
        var properties = await _masterService.GetCurrentFlightValuesAsync();
        res.Longitude = properties.Longitude;
        res.Latitude = properties.Latitude;
        res.Altitude = properties.Altitude;
        res.AltitudeAgl = properties.AltitudeAgl;
        res.Roll = properties.Roll;
        res.Pitch = properties.Pitch;
        res.Heading = properties.Heading;
        res.IndicatedSpeed = properties.IndicatedSpeed;
        res.Airspeed = properties.Airspeed;
        return Ok(res);
    }
    
    /// <summary>
    /// Досрочный выход из симуляции полета
    /// </summary>
    [HttpPost("exit")]
    [ProducesResponseType(typeof(BasicStatusResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BasicStatusResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ExitSimulation()
    {
        await _masterService.ExitSimulationWithPropertySaveAsync();
        return Ok(new BasicStatusResponse
        {
            Status = BasicStatusEnum.Success.ToString(),
            Comment = "Симуляция завершена."
        });
    }
}