using Application.Interfaces;
using Application.Interfaces.Entities;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using webapi.Controllers;
using WebApi.Controllers.Base;
using WebApi.Controllers.FlightPlans.Requests;
using WebApi.Controllers.FlightPlans.Responses;

namespace WebApi.Controllers.FlightPlans;

[Route("/api/flightplans")]
public class FlightPlansController : Controller
{
    private readonly IFlightPlanService _flightPlanService;

    public FlightPlansController(IFlightPlanService flightPlanService)
    {
        _flightPlanService = flightPlanService;
    }

    /// <summary>
    /// Получение базовой информации о всех сохраненных планах полета
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(GetAllFlightPlansResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BasicStatusResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetAllFlightPlansBasicInfo()
    {
        var flightPlans = await _flightPlanService.GetAllBasicFlightPlansInfosAsync();
        var res = DtoConverter.ConvertFlightPlansArrayToBasicInfoResponse(flightPlans);
        return Ok(res);
    }

    /// <summary>
    /// Получение полной информации о сохраненненном плане полета по идентификатору
    /// </summary>
    /// <param name="flightPlanId">Уникальный идентификатор плана полета.</param>
    [HttpGet("{flightPlanId:guid}")]
    [ProducesResponseType(typeof(FlightPlanResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BasicStatusResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetFlightPlanInfo([FromRoute] Guid flightPlanId)
    {
        var flightPlan = await _flightPlanService.GetFlightPlanWithConvertedPointsAsync(flightPlanId);
        var res = DtoConverter.ConvertAggregatedFlightPlanToResponse(flightPlan);
        return Ok(res);
    }

    /// <summary>
    /// Удаление плана полета по уникальному идентификатору
    /// </summary>
    /// <param name="flightPlanId">Уникальный идентификатор плана полет</param>
    [HttpDelete("{flightPlanId:guid}")]
    [ProducesResponseType(typeof(BasicStatusResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BasicStatusResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteFlightPlan([FromRoute] Guid flightPlanId)
    {
        await _flightPlanService.RemoveFlightPlanAsync(flightPlanId);
        return Ok(new BasicStatusResponse
        {
            Status = BasicStatusEnum.Success.ToString(),
            Comment = "План полета успешно удален."
        });
    }

    /// <summary>
    /// Добавление нового плана полета
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(FlightPlanWithStatusResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BasicStatusResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateNewFlightPlan([FromBody] CreateFlightPlanRequest dto)
    {
        var id = Guid.NewGuid();
        var flightPlan = new FlightPlan
        {
            Title = dto.Title,
            Remarks = dto.Remarks,
            DepartureRunwayId = dto.DepartureRunwayId,
            ArrivalRunwayId = dto.ArrivalRunwayId,
            Id = id
        };
        await _flightPlanService.SaveFlightPlanAsync(flightPlan);
        var savedFlightPlan = await _flightPlanService.GetFlightPlanWithConvertedPointsAsync(id);
        return Ok(new FlightPlanWithStatusResponse
        {
            Status = BasicStatusEnum.Success.ToString(),
            Comment = "План полета успешно создан.",
            FlightPlan = DtoConverter.ConvertAggregatedFlightPlanToResponse(savedFlightPlan)
        });
    }

    /// <summary>
    /// Обновление информации в плана полета
    /// </summary>
    /// <param name="flightPlanId">Уникальный идентификатор плана полет</param>
    /// <param name="dto">Модель с обновленными данными</param>
    [HttpPut("{flightPlanId:guid}")]
    [ProducesResponseType(typeof(FlightPlanWithStatusResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BasicStatusResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteFlightPlan([FromRoute] Guid flightPlanId, [FromBody] UpdateFlightPlanRequest dto)
    {
        var flightPlan = await _flightPlanService.GetAggregatedFlightPlanAsync(flightPlanId);
        flightPlan.Title = dto.Title;
        flightPlan.Remarks = dto.Remarks;
        flightPlan.DepartureRunwayId = dto.DepartureRunwayId;
        flightPlan.ArrivalRunwayId = dto.ArrivalRunwayId;
        await _flightPlanService.SaveFlightPlanAsync(flightPlan);
        var savedFlightPlan = await _flightPlanService.GetFlightPlanWithConvertedPointsAsync(flightPlan.Id);
        return Ok(new FlightPlanWithStatusResponse
        {
            Status = BasicStatusEnum.Success.ToString(),
            Comment = "План полета успешно обновлен.",
            FlightPlan = DtoConverter.ConvertAggregatedFlightPlanToResponse(savedFlightPlan)
        });
    }

    /// <summary>
    /// Получение полного списка точек маршрута по уникальному идентификатору плана полета
    /// </summary>
    /// <param name="flightPlanId">Уникальный идентификатор плана полет</param>
    [HttpGet("{flightPlanId:guid}/points")]
    [ProducesResponseType(typeof(GetFlightPlanPointsResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BasicStatusResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetFlightPlanPoints([FromRoute] Guid flightPlanId)
    {
        var flightPlan = await _flightPlanService.GetFlightPlanWithConvertedPointsAsync(flightPlanId);
        var array = DtoConverter.ConvertAggregatedFlightPlanToRoutePointsResponseArray(flightPlan);
        return Ok(new GetFlightPlanPointsResponse
        {
            RoutePoints = array
        });
    }
    
    /// <summary>
    /// Добавление новой точки маршрута к плану полета
    /// </summary>
    /// <param name="flightPlanId">Уникальный идентификатор плана полет</param>
    [HttpPost("{flightPlanId:guid}/points")]
    [ProducesResponseType(typeof(FlightPlanWithStatusResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BasicStatusResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddRoutePointToFlightPlan([FromRoute] Guid flightPlanId, [FromBody] AddRoutePointRequest dto)
    {
        var flightPlan = await _flightPlanService.GetAggregatedFlightPlanAsync(flightPlanId);
        var routePoint = new RoutePoint
        {
            Order = flightPlan.RoutePoints.Count,
            Longitude = dto.Longitude,
            Latitude = dto.Latitude,
            Altitude = dto.Altitude,
            Remarks = dto.Remarks,
            FlightPlanId = flightPlanId,
            Id = Guid.NewGuid()
        };
        await _flightPlanService.SaveRoutePointAsync(routePoint, flightPlan);
        
        var flightPlanUpdated = await _flightPlanService.GetFlightPlanWithConvertedPointsAsync(flightPlanId);
        var res = DtoConverter.ConvertAggregatedFlightPlanToResponse(flightPlanUpdated);
        return Ok(new FlightPlanWithStatusResponse
        {
            Status = BasicStatusEnum.Success.ToString(),
            Comment = "Новая точка маршрута успешно добавлена.",
            FlightPlan = res
        });
    }
    
    /// <summary>
    /// Удаление точки маршрута у плана полета
    /// </summary>
    /// <param name="flightPlanId">Уникальный идентификатор плана полет</param>
    /// <param name="pointId">Уникальный идентификатор точки маршрута, которую нужно удалить.</param>
    [HttpDelete("{flightPlanId:guid}/points/{pointId:guid}")]
    [ProducesResponseType(typeof(FlightPlanWithStatusResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BasicStatusResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteRoutePointFromFlightPlan([FromRoute] Guid flightPlanId, [FromRoute] Guid pointId)
    {
        await _flightPlanService.RemoveRoutePointAsync(flightPlanId, pointId);
        
        var flightPlanUpdated = await _flightPlanService.GetFlightPlanWithConvertedPointsAsync(flightPlanId);
        var res = DtoConverter.ConvertAggregatedFlightPlanToResponse(flightPlanUpdated);
        return Ok(new FlightPlanWithStatusResponse
        {
            Status = BasicStatusEnum.Success.ToString(),
            Comment = "Точка маршрута была успешно удалена.",
            FlightPlan = res
        });
    }
    
    /// <summary>
    /// Обновление точки маршрута у существующего плана полета
    /// </summary>
    /// <param name="flightPlanId">Уникальный идентификатор плана полет.</param>
    /// <param name="pointId">Уникальный идентификатор точки маршрута, которую нужно обновить.</param>
    /// <param name="dto">Модель с новыми значениями для точки маршрута.</param>
    [HttpPut("{flightPlanId:guid}/points/{pointId:guid}")]
    [ProducesResponseType(typeof(FlightPlanWithStatusResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BasicStatusResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateRoutePointInFlightPlan([FromRoute] Guid flightPlanId, [FromRoute] Guid pointId,
        [FromBody] UpdateRoutePointRequest dto)
    {
        await _flightPlanService.UpdateRoutePointAsync(flightPlanId, pointId, dto.Longitude, dto.Latitude, dto.Altitude, dto.Remarks);
        
        var flightPlanUpdated = await _flightPlanService.GetFlightPlanWithConvertedPointsAsync(flightPlanId);
        var res = DtoConverter.ConvertAggregatedFlightPlanToResponse(flightPlanUpdated);
        return Ok(new FlightPlanWithStatusResponse
        {
            Status = BasicStatusEnum.Success.ToString(),
            Comment = "Точка маршрута была успешно обновлена.",
            FlightPlan = res
        });
    }
}