using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[Route("api/analytics")]
public class AnalyticsController : Controller
{
    private readonly IFlightExportParametersManager _flightExportParametersManager;

    public AnalyticsController(IFlightExportParametersManager flightExportParametersManager)
    {
        _flightExportParametersManager = flightExportParametersManager;
    }
    
    /// <summary>
    /// Пример реста по GET запросу на /api/analytics/sessions
    /// </summary>
    [HttpGet("sessions")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetSessions()
    {
        return Ok("OK!");
    }
}