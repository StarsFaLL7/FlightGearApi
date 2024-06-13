using Application.Dto;
using Application.Interfaces;
using Application.Interfaces.Entities;
using Application.Interfaces.Repositories;
using Domain.Entities;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Services.Entities;

/// <inheritdoc />
internal class FlightPlanService : IFlightPlanService
{
    private readonly IRunwayService _runwayService;
    private readonly IServiceProvider _serviceProvider;

    public FlightPlanService(IRunwayService runwayService, IServiceProvider serviceProvider)
    {
        _runwayService = runwayService;
        _serviceProvider = serviceProvider;
    }
    
    public async Task<FlightPlan[]> GetAllBasicFlightPlansInfosAsync()
    {
        var flightPlanRepository = _serviceProvider.GetRequiredService<IFlightPlanRepository>();
        return await flightPlanRepository.GetAll();
    }

    public async Task<FlightPlan> GetAggregatedFlightPlanAsync(Guid flightPlanId)
    {
        var flightPlanRepository = _serviceProvider.GetRequiredService<IFlightPlanRepository>();
        return await flightPlanRepository.GetAggregateByIdAsync(flightPlanId);
    }

    public async Task<FlightPlanAggregated> GetFlightPlanWithConvertedPointsAsync(Guid flightPlanId)
    {
        var flightPlanRepository = _serviceProvider.GetRequiredService<IFlightPlanRepository>();
        var flightPlan = await flightPlanRepository.GetAggregateByIdAsync(flightPlanId);
        var result = new FlightPlanAggregated
        {
            Title = flightPlan.Title,
            DepartureRunway = null,
            ArrivalRunway = null,
            Remarks = flightPlan.Remarks,
            RoutePoints = new List<RoutePointWithIsEditableProperty>(),
            Id = flightPlan.Id
        };
        if (flightPlan.DepartureRunwayId != null)
        {
            var departureRunway = await _runwayService.GetAggregatedRunwayByIdAsync(flightPlan.DepartureRunwayId.Value);
            result.DepartureRunway = departureRunway;
            if (departureRunway.DepartureFunction == null)
            {
                throw new Exception($"В плане полета \"{flightPlanId}\" указана взлетная полоса, для которой отсутствует функция взлета.");
            }
            foreach (var functionPoint in departureRunway.DepartureFunction.FunctionPoints)
            {
                result.RoutePoints.Add(new RoutePointWithIsEditableProperty
                {
                    Order = result.RoutePoints.Count,
                    Longitude = functionPoint.Longitude,
                    Latitude = functionPoint.Latitude,
                    Remarks = functionPoint.Remarks,
                    Altitude = functionPoint.Altitude,
                    Id = functionPoint.Id,
                    IsEditable = false
                });
            }
        }

        foreach (var userPoint in flightPlan.RoutePoints.OrderBy(p => p.Order))
        {
            result.RoutePoints.Add(new RoutePointWithIsEditableProperty
            {
                Order = result.RoutePoints.Count,
                Longitude = userPoint.Longitude,
                Latitude = userPoint.Latitude,
                Altitude = userPoint.Altitude,
                Remarks = userPoint.Remarks,
                Id = userPoint.Id,
                IsEditable = true
            });
        }
        
        if (flightPlan.ArrivalRunwayId != null)
        {
            var arrivalRunway = await _runwayService.GetAggregatedRunwayByIdAsync(flightPlan.ArrivalRunwayId.Value);
            result.ArrivalRunway = arrivalRunway;
            if (arrivalRunway.ArrivalFunction == null)
            {
                throw new Exception($"В плане полета \"{flightPlanId}\" указана посадочная полоса, для которой отсутствует функция посадки.");
            }
            foreach (var functionPoint in arrivalRunway.ArrivalFunction.FunctionPoints)
            {
                result.RoutePoints.Add(new RoutePointWithIsEditableProperty
                {
                    Order = result.RoutePoints.Count,
                    Longitude = functionPoint.Longitude,
                    Latitude = functionPoint.Latitude,
                    Altitude = functionPoint.Altitude,
                    Remarks = functionPoint.Remarks,
                    Id = functionPoint.Id,
                    IsEditable = false
                });
            }
        }

        return result;
    }

    public async Task<FlightPlan> SaveFlightPlanAsync(FlightPlan flightPlan)
    {
        var flightPlanRepository = _serviceProvider.GetRequiredService<IFlightPlanRepository>();
        await flightPlanRepository.SaveAsync(flightPlan);
        return await flightPlanRepository.GetAggregateByIdAsync(flightPlan.Id);
    }

    public async Task RemoveFlightPlanAsync(Guid flightPlanId)
    {
        var flightPlanRepository = _serviceProvider.GetRequiredService<IFlightPlanRepository>();
        await flightPlanRepository.RemoveByIdAsync(flightPlanId);
    }

    public async Task UpdateRoutePointAsync(Guid flightPlanId, Guid pointId, double longitude, double latitude, double altitude,
        string? remarks)
    {
        if (pointId == Guid.Empty)
        {
            throw new Exception("Данную точку нельзя редактировать.");
        }
        var flightPlan = await GetAggregatedFlightPlanAsync(flightPlanId);
        var point = flightPlan.RoutePoints.FirstOrDefault(p => p.Id == pointId);
        if (point == null)
        {
            throw new Exception("Точки маршрута с указанным id нет в данном плане полёта.");
        }
        point.Longitude = longitude;
        point.Latitude = latitude;
        point.Altitude = altitude;
        point.Remarks = remarks;
        await SaveRoutePointAsync(point, flightPlan);
    }

    public async Task SaveRoutePointAsync(RoutePoint routePoint, FlightPlan flightPlan)
    {
        var routePointRepository = _serviceProvider.GetRequiredService<IRoutePointRepository>();
        var flightPlanRepository = _serviceProvider.GetRequiredService<IFlightPlanRepository>();
        await flightPlanRepository.SaveAsync(flightPlan);
        await routePointRepository.SaveAsync(routePoint);
    }

    public async Task RemoveRoutePointAsync(Guid flightPlanId, Guid pointId)
    {
        if (pointId == Guid.Empty)
        {
            throw new Exception("Данную точку нельзя редактировать.");
        }
        var flightPlanRepository = _serviceProvider.GetRequiredService<IFlightPlanRepository>();
        var routePointRepository = _serviceProvider.GetRequiredService<IRoutePointRepository>();
        
        var flightPlan = await GetAggregatedFlightPlanAsync(flightPlanId);
        var point = flightPlan.RoutePoints.FirstOrDefault(p => p.Id == pointId);
        if (point == null)
        {
            throw new Exception("Точки маршрута с указанным id нет в данном плане полёта.");
        }
        foreach (var planPoint in flightPlan.RoutePoints.Where(p => p.Order > point.Order))
        {
            planPoint.Order -= 1;
            await routePointRepository.SaveAsync(planPoint);
        }
        flightPlan.RoutePoints.Remove(point);
        await flightPlanRepository.SaveAsync(flightPlan);
    }

    public async Task RemoveDepartureRunwayFromFlightPlansByRunwayId(Guid runwayId)
    {        
        var flightPlanRepository = _serviceProvider.GetRequiredService<IFlightPlanRepository>();

        var flightplans = await flightPlanRepository.GetFlightPlansByDepartureRunwayId(runwayId);
        foreach (var flightplan in flightplans)
        {
            flightplan.DepartureRunway = null;
            flightplan.DepartureRunwayId = null;
            await flightPlanRepository.SaveAsync(flightplan);
        }
    }

    public async Task RemoveArrivalRunwayFromFlightPlansByRunwayId(Guid runwayId)
    {
        var flightPlanRepository = _serviceProvider.GetRequiredService<IFlightPlanRepository>();
        var flightplans = await flightPlanRepository.GetFlightPlansByArrivalRunwayId(runwayId);
        foreach (var flightplan in flightplans)
        {
            flightplan.ArrivalRunway = null;
            flightplan.ArrivalRunwayId = null;
            await flightPlanRepository.SaveAsync(flightplan);
        }
    }
}