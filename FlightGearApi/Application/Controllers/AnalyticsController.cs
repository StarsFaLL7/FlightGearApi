using FlightGearApi.Application.DTO;
using FlightGearApi.Domain.Enums;
using FlightGearApi.Infrastructure.Interfaces;
using FlightGearApi.Infrastructure.ModelsDal;
using Microsoft.AspNetCore.Mvc;

namespace FlightGearApi.Application.Controllers;

[Route("api/analytics")]
public class AnalyticsController : Controller
{
    private readonly IPostgresDatabase _database;
    
    public AnalyticsController(IPostgresDatabase database)
    {
        _database = database;
    }
    
    /// <summary>
    /// Получить список всех сессий, сохранённых в БД, без списка параметров (null)
    /// </summary>
    /// <returns></returns>
    [HttpGet("sessions")]
    [ProducesResponseType(typeof(List<FlightSessionDal>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetSessions()
    {
        return Ok(_database.GetAllSessions());
    }
    
    /// <summary>
    /// Получить список параметров и кол-во их считываний для сессии с id = sessionId
    /// </summary>
    /// <param name="sessionId">Id сессии, параметры которой нужно получить</param>
    /// <returns></returns>
    [HttpGet("sessions/{sessionId:int}")]
    [ProducesResponseType(typeof(IEnumerable<FlightPropertyInfoDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetPropertiesList(int sessionId)
    {
        var session = _database.GetSessionWithProperties(sessionId);
        if (session == null)
        {
            return NotFound("No session with given id found.");
        }
        var result = ExportPropertyExtensions.PropertiesInfoDict.Select(p => new FlightPropertyInfoDto()
            { Name = p.Value.RussianString, Count = session.PropertiesCollection.Count });
        return Ok(result);
    }
    
    /// <summary>
    /// Получить значения считываний параметров для сессии с id = sessionId
    /// </summary>
    /// <param name="sessionId">Id сессии, значения параметров которой нужно получить</param>
    /// <returns></returns>
    [HttpGet("sessions/{sessionId:int}/values")]
    [ProducesResponseType(typeof(List<PropertiesValuesResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetPropertiesValues(int sessionId)
    {
        var session = _database.GetSessionWithProperties(sessionId);
        if (session == null)
        {
            return NotFound("No session with given id found.");
        }
        var result = _database.GetPropertiesValuesResponseList(session);
        return Ok(result);
    }
}