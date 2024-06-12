using Application.Interfaces.Entities;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using webapi.Controllers;
using WebApi.Controllers.Airports.Requests;
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
            Airports = airports.Select(airport => new AirportBasicInfoResponse
            {
                Id = airport.Id,
                Title = airport.Title,
                Code = airport.Code,
                City = airport.City
            }).ToArray()
        };
        return Ok(res);
    }
    
    /// <summary>
    /// Получение полной информации об аэропорте, по его уникальному идентификатору
    /// </summary>
    /// <param name="airportId">Уникальный идентификатор аэропорта.</param>
    [HttpGet("{airportId:guid}")]
    [ProducesResponseType(typeof(AirportResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BasicStatusResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetAirportInfo([FromRoute] Guid airportId)
    {
        var airport = await _airportService.GetAirportAggregatedAsync(airportId);

        var res = DtoConverter.ConvertAggregatedAirportToAirportResponse(airport);
        return Ok(res);
    }
    
    /// <summary>
    /// Добавление нового аэропорта
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(AirportResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BasicStatusResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateAirport([FromBody] CreateAirportRequest dto)
    {
        var airport = new Airport
        {
            Title = dto.Title,
            Code = dto.Code,
            City = dto.City,
            Id = Guid.NewGuid()
        };
        await _airportService.SaveAirportAsync(airport);
        var res = DtoConverter.ConvertAggregatedAirportToAirportResponse(airport);
        return Ok(res);
    }
    
    /// <summary>
    /// Удаление аэропорта со всеми взлетными полосами
    /// </summary>
    /// <param name="airportId">Уникальный идентификатор аэропорта.</param>
    [HttpDelete("{airportId:guid}")]
    [ProducesResponseType(typeof(BasicStatusResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BasicStatusResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteAirport([FromRoute] Guid airportId)
    {
        await _airportService.DeleteAirportAsync(airportId);
        return Ok(new BasicStatusResponse
        {
            Status = BasicStatusEnum.Success.ToString(),
            Comment = "Аэропорт успешно удален."
        });
    }
    
    /// <summary>
    /// Обновление данных аэропорта
    /// </summary>
    /// <param name="airportId">Уникальный идентификатор аэропорта.</param>
    [HttpPut("{airportId:guid}")]
    [ProducesResponseType(typeof(AirportResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BasicStatusResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateAirport([FromRoute] Guid airportId, [FromBody] UpdateAirportRequest dto)
    {
        var airport = await _airportService.GetAirportAggregatedAsync(airportId);
        airport.City = dto.City;
        airport.Code = dto.Code;
        airport.Title = dto.Title;
        await _airportService.SaveAirportAsync(airport);
        var res = DtoConverter.ConvertAggregatedAirportToAirportResponse(airport);
        return Ok(res);
    }
}