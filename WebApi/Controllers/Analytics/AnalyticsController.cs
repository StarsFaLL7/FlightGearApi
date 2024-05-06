using Application.Interfaces.Entities;
using Microsoft.AspNetCore.Mvc;
using webapi.Controllers.Analytics.Responses;
using WebApi.Controllers.Base;
using WebApi.Controllers.Responses;

namespace webapi.Controllers.Analytics;

[Route("api/analytics")]
public class AnalyticsController : Controller
{
    private readonly ISessionService _sessionService;

    public AnalyticsController(ISessionService sessionService)
    {
        _sessionService = sessionService;
    }
    
    /// <summary>
    /// Получить список всех сохраненных сессий полета
    /// </summary>
    [HttpGet("sessions/")]
    [ProducesResponseType(typeof(GetAllSessionsResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllSessions()
    {
        var sessions = await _sessionService.GetAllSessions();
        var res = new GetAllSessionsResponse
        {
            Sessions = sessions.Select(s => new SessionBasicResponse
            {
                Id = s.Id,
                Title = s.Title,
                PropertiesReadsPerSec = s.PropertiesReadsPerSec,
                DateTimeStart = s.DateTimeStart,
                DurationSec = s.DurationSec,
                DateTimeEnd = s.DateTimeEnd
            }).ToArray()
        };
        return Ok(res);
    }
    
    /// <summary>
    /// Получить значения, записанные в течение сессии с указанным уникальный идентификатором
    /// </summary>
    [HttpGet("sessions/{sessionId:guid}")]
    [ProducesResponseType(typeof(SessionFullResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetSessionInfo([FromRoute] Guid sessionId)
    {
        var session = await _sessionService.GetAggregatedSession(sessionId);
        var res = new SessionFullResponse
        {
            Properties = DtoConverter.ConvertPropertyShotsToPropertiesResponseArray(session.PropertiesCollection.ToArray()),
            Id = session.Id,
            Title = session.Title,
            PropertiesReadsPerSec = session.PropertiesReadsPerSec,
            DateTimeStart = session.DateTimeStart,
            DurationSec = session.DurationSec,
            DateTimeEnd = session.DateTimeEnd
        };
        return Ok(res);
    }
    
    /// <summary>
    /// Удалить сессию с указанным уникальный идентификатором
    /// </summary>
    [HttpDelete("sessions/{sessionId:guid}")]
    [ProducesResponseType(typeof(BasicStatusResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteSession([FromRoute] Guid sessionId)
    {
        await _sessionService.RemoveSessionAsync(sessionId);
        var res = new BasicStatusResponse
        {
            Status = BasicStatusEnum.Success.ToString(),
            Comment = "Сессия успешно удалена."
        };
        return Ok(res);
    }
    
}