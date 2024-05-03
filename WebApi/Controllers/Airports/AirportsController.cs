using Application.Interfaces.Entities;
using Microsoft.AspNetCore.Mvc;
using webapi.Controllers;
using WebApi.Controllers.Airports.Responses;
using WebApi.Controllers.Base;

namespace WebApi.Controllers.Airports;

[Route("/api/airports")]
public class AirportsController : Controller
{
    private readonly IAirportService _airportService;

    public AirportsController(IAirportService airportService)
    {
        _airportService = airportService;
    }
    
    /// <summary>
    /// Получение базовой информации о всех аэропортах, сохраненных в базе данных
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(GetAllAirportsResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BasicStatusResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetAllAirports()
    {
        var airports = await _airportService.GetAllAirportsAsync();
        
        var res = new GetAllAirportsResponse
        {
            Airports = airports.Select(a => new AirportBasicInfoResponse
            {
                Id = a.Id,
                Title = a.Title,
                Code = a.Code,
                City = a.City
            }).ToArray()
        };
        return Ok(res);
    }
    
    /// <summary>
    /// Получение полной информации об аэропорте, по его уникальному идентификатору
    /// </summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(GetAllAirportsResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BasicStatusResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetAirportInfo([FromRoute] Guid id)
    {
        var airport = await _airportService.GetAirportAggregatedAsync(id);

        var res = DtoConverter.ConvertAggregatedAirportToAirportResponse(airport);
        return Ok(res);
    }
}