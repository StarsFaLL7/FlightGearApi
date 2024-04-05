using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using webapi.DtoModels;

namespace WebApi.Controllers;

[Route("api/analytics")]
public class AnalyticsController : Controller
{
    private readonly IFlightExportParametersManager _flightExportParametersManager;

    public AnalyticsController(IFlightExportParametersManager flightExportParametersManager) // Инжектируем сервис через DI
    {
        _flightExportParametersManager = flightExportParametersManager;
    }
    
    /// <summary>
    /// Пример реста по GET запросу на /api/analytics/sessions/{число}
    /// </summary>
    [HttpGet("sessions/{id:int}")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetSessions([FromQuery] int id)
    {
        return Ok($"OK! You entered id: {id}");
    }
    
    /// <summary>
    /// Пример реста по Post запросу на /api/analytics/dto-test
    /// Действие ожидает получить TestDtoRequest в body запроса. Если у него не получится смаппить его в объект, ответом вернётся ошибка.
    /// </summary>
    [HttpPost("dto-test")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    public async Task<IActionResult> CreateSession([FromBody] TestDtoRequest dtoRequest)
    {
        dtoRequest.Number = Random.Shared.Next(1000);
        return Ok($"OK! We changed id in dtoModel to random number: {dtoRequest}");
    }
}