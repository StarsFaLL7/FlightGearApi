using Application.Interfaces.Entities;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using WebApi.Controllers.Airports.Responses;
using WebApi.Controllers.Base;
using webapi.Controllers.Runways.Requests;
using webapi.Controllers.Runways.Responses;

namespace webapi.Controllers.Runways;

[Route("api/runways")]
public class RunwayController : Controller
{
    private readonly IRunwayService _runwayService;
    private readonly IFlightFunctionService _functionService;
    private readonly IFlightPlanService _flightPlanService;

    public RunwayController(IRunwayService runwayService, IFlightFunctionService functionService,
        IFlightPlanService flightPlanService)
    {
        _runwayService = runwayService;
        _functionService = functionService;
        _flightPlanService = flightPlanService;
    }
    
    /// <summary>
    /// Получение всех взлетных полос по уникальному идентификатору аэропорта
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(AirportResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BasicStatusResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetRunwaysByAirportId([FromQuery] Guid airportId)
    {
        var runways = await _runwayService.GetAllRunwaysByAirportId(airportId);
        var res = new GetRunwaysListResponse
        {
            Runways = runways.Select(r => new RunwayBasicInfoResponse
            {
                Id = r.Id,
                Title = r.Title,
                CanBeDeparture = r.DepartureFunctionId != null,
                CanBeArrival = r.ArrivalFunctionId != null
            }).ToArray()
        };
        return Ok(res);
    }
    
    /// <summary>
    /// Получение полной информации о взлетной полосе по уникальному идентификатору
    /// </summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(RunwayFullResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BasicStatusResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetRunwayById([FromRoute] Guid id)
    {
        var runway = await _runwayService.GetAggregatedRunwayByIdAsync(id);
        var res = DtoConverter.ConvertAggregatedRunwayToFullResponse(runway);
        return Ok(res);
    }
    
    /// <summary>
    /// Добавление взлетной полосы
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(RunwayFullResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BasicStatusResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateRunway([FromBody] CreateRunwayRequest dto)
    {
        var runway = new AirportRunway
        {
            Title = dto.Title,
            AirportId = dto.AirportId,
            DepartureFunctionId = null,
            DepartureFunction = null,
            ArrivalFunctionId = null,
            ArrivalFunction = null,
            Id = Guid.NewGuid()
        };
        await _runwayService.SaveRunwayAsync(runway);
        var runwayResult = await _runwayService.GetAggregatedRunwayByIdAsync(runway.Id);
        var res = DtoConverter.ConvertAggregatedRunwayToFullResponse(runwayResult);
        return Ok(res);
    }
    
    /// <summary>
    /// Удаление взлетной полосы по уникальному идентификатору
    /// </summary>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(typeof(BasicStatusResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BasicStatusResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteRunwayById([FromRoute] Guid id)
    {
        await _runwayService.RemoveRunwayByIdAsync(id);
        return Ok(new BasicStatusResponse
        {
            Status = BasicStatusEnum.Success.ToString(),
            Comment = "Взлетная полоса успешно удалена."
        });
    }
    
    /// <summary>
    /// Добавление функции взлета для взлетной полосы
    /// </summary>
    [HttpPost("{runwayId:guid}/departure")]
    [ProducesResponseType(typeof(RunwayFullResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BasicStatusResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateDepartureFunction([FromRoute] Guid runwayId, [FromBody] CreateFunctionRequest dto)
    {
        var runway = await _runwayService.GetRunwayByIdAsync(runwayId);
        if (runway.DepartureFunctionId != null)
        {
            throw new Exception("У данной взлетной полосы уже есть функция взлета. Сначала удалите ее, чтобы создать новую.");
        }

        var funcId = Guid.NewGuid();
        var departureFunction = new ReadyFlightFunction
        {
            Id = funcId,
            Description = dto.Description,
            FunctionPoints = dto.Points.Select((p, i) => new FunctionPoint
            {
                Order = i,
                Longitude = p.Longitude,
                Latitude = p.Latitude,
                Altitude = p.Altitude,
                Speed = p.Speed,
                Remarks = p.Remarks,
                FunctionId = funcId,
                Id = Guid.NewGuid()
            }).ToArray()
        };
        await _functionService.SaveFunction(departureFunction);
        runway.DepartureFunctionId = funcId;
        await _runwayService.SaveRunwayAsync(runway);
        var runwayResult = await _runwayService.GetAggregatedRunwayByIdAsync(runway.Id);
        var res = DtoConverter.ConvertAggregatedRunwayToFullResponse(runwayResult);
        return Ok(res);
    }
    
    /// <summary>
    /// Добавление функции посадки для взлетной полосы
    /// </summary>
    [HttpPost("{runwayId:guid}/arrival")]
    [ProducesResponseType(typeof(RunwayFullResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BasicStatusResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateArrivalFunction([FromRoute] Guid runwayId, [FromBody] CreateFunctionRequest dto)
    {
        var runway = await _runwayService.GetRunwayByIdAsync(runwayId);
        if (runway.ArrivalFunctionId != null)
        {
            throw new Exception("У данной взлетной полосы уже есть функция посадки. Сначала удалите ее, чтобы создать новую.");
        }

        var funcId = Guid.NewGuid();
        var arrivalFunc = new ReadyFlightFunction
        {
            Id = funcId,
            Description = dto.Description,
            FunctionPoints = dto.Points.Select((p, i) => new FunctionPoint
            {
                Order = i,
                Longitude = p.Longitude,
                Latitude = p.Latitude,
                Altitude = p.Altitude,
                Speed = p.Speed,
                Remarks = p.Remarks,
                FunctionId = funcId,
                Id = Guid.NewGuid()
            }).ToArray()
        };
        await _functionService.SaveFunctionPointRange(arrivalFunc.FunctionPoints.ToArray());
        await _functionService.SaveFunction(arrivalFunc);
        runway.ArrivalFunctionId = funcId;
        await _runwayService.SaveRunwayAsync(runway);
        
        var runwayResult = await _runwayService.GetAggregatedRunwayByIdAsync(runway.Id);
        var res = DtoConverter.ConvertAggregatedRunwayToFullResponse(runwayResult);
        return Ok(res);
    }
    
    /// <summary>
    /// Удаление функции взлета у взлетной полосы
    /// </summary>
    [HttpDelete("{runwayId:guid}/departure")]
    [ProducesResponseType(typeof(RunwayFullResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BasicStatusResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteDepartureFunction([FromRoute] Guid runwayId)
    {
        var runway = await _runwayService.GetRunwayByIdAsync(runwayId);
        if (runway.DepartureFunctionId == null)
        {
            throw new Exception("У данной взлетной полосы отсутствует функция взлета.");
        }

        var funcId = runway.DepartureFunctionId.Value;
        runway.DepartureFunctionId = null;
        runway.DepartureFunction = null;
        await _runwayService.SaveRunwayAsync(runway);
        
        await _functionService.RemoveFunctionById(funcId);
        await _flightPlanService.RemoveDepartureRunwayFromFlightPlansByRunwayId(runway.Id);
        
        var runwayResult = await _runwayService.GetAggregatedRunwayByIdAsync(runway.Id);
        var res = DtoConverter.ConvertAggregatedRunwayToFullResponse(runwayResult);
        return Ok(res);
    }
    
    /// <summary>
    /// Удаление функции посадки у взлетной полосы
    /// </summary>
    [HttpDelete("{runwayId:guid}/arrival")]
    [ProducesResponseType(typeof(RunwayFullResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BasicStatusResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteArrivalFunction([FromRoute] Guid runwayId)
    {
        var runway = await _runwayService.GetRunwayByIdAsync(runwayId);
        if (runway.ArrivalFunctionId == null)
        {
            throw new Exception("У данной взлетной полосы отсутствует функция взлета.");
        }
        var funcId = runway.ArrivalFunctionId.Value;
        runway.ArrivalFunctionId = null;
        runway.ArrivalFunction = null;
        await _runwayService.SaveRunwayAsync(runway);
        
        await _functionService.RemoveFunctionById(funcId);
        await _flightPlanService.RemoveArrivalRunwayFromFlightPlansByRunwayId(runway.Id);
        
        var runwayResult = await _runwayService.GetAggregatedRunwayByIdAsync(runway.Id);
        var res = DtoConverter.ConvertAggregatedRunwayToFullResponse(runwayResult);
        return Ok(res);
    }
}