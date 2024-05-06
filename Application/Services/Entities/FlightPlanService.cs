﻿using Application.Dto;
using Application.Interfaces;
using Application.Interfaces.Entities;
using Application.Interfaces.Repositories;
using Domain.Entities;

namespace Application.Services.Entities;

/// <inheritdoc />
internal class FlightPlanService : IFlightPlanService
{
    private readonly IFlightPlanRepository _flightPlanRepository;
    private readonly IRunwayService _runwayService;
    private readonly IRoutePointRepository _routePointRepository;

    public FlightPlanService(IFlightPlanRepository flightPlanRepository, IRunwayService runwayService, 
        IRoutePointRepository routePointRepository)
    {
        _flightPlanRepository = flightPlanRepository;
        _runwayService = runwayService;
        _routePointRepository = routePointRepository;
    }
    
    public async Task<FlightPlan[]> GetAllBasicFlightPlansInfosAsync()
    {
        return await _flightPlanRepository.GetAll();
    }

    public async Task<FlightPlan> GetAggregatedFlightPlanAsync(Guid flightPlanId)
    {
        return await _flightPlanRepository.GetAggregateByIdAsync(flightPlanId);
    }

    public async Task<FlightPlanAggregated> GetFlightPlanWithConvertedPointsAsync(Guid flightPlanId)
    {
        var flightPlan = await _flightPlanRepository.GetAggregateByIdAsync(flightPlanId);
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
                    Id = Guid.Empty,
                    IsEditable = false
                });
            }
        }

        foreach (var userPoint in flightPlan.RoutePoints)
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
                    Id = Guid.Empty,
                    IsEditable = false
                });
            }
        }

        return result;
    }

    public async Task<FlightPlan> SaveFlightPlanAsync(FlightPlan flightPlan)
    {
        await _flightPlanRepository.SaveAsync(flightPlan);
        return await _flightPlanRepository.GetAggregateByIdAsync(flightPlan.Id);
    }

    public async Task RemoveFlightPlanAsync(Guid flightPlanId)
    {
        await _flightPlanRepository.RemoveByIdAsync(flightPlanId);
    }

    public async Task UpdateRoutePointAsync(Guid flightPlanId, int pointOrder, double longitude, double latitude, double altitude,
        string? remarks)
    {
        var flightPlan = await GetAggregatedFlightPlanAsync(flightPlanId);
        if (pointOrder < 0 || pointOrder > flightPlan.RoutePoints.Count - 1)
        {
            throw new ArgumentException(
                $"Указано некорректное значение порядкового номера точки маршрута. Кол-во точек в маршруте: {flightPlan.RoutePoints.Count}");
        }
        var point = flightPlan.RoutePoints[pointOrder];
        point.Longitude = longitude;
        point.Latitude = latitude;
        point.Altitude = altitude;
        point.Remarks = remarks;
        await SaveRoutePointAsync(point);
    }

    public async Task SaveRoutePointAsync(RoutePoint routePoint)
    {
        await _routePointRepository.SaveAsync(routePoint);
    }

    public async Task RemoveRoutePointAsync(Guid flightPlanId, int pointOrder)
    {
        var flightPlan = await GetAggregatedFlightPlanAsync(flightPlanId);
        if (pointOrder < 0 || pointOrder > flightPlan.RoutePoints.Count - 1)
        {
            throw new ArgumentException(
                $"Указано некорректное значение порядкового номера точки маршрута. Кол-во точек в маршруте: {flightPlan.RoutePoints.Count}");
        }
        var point = flightPlan.RoutePoints[pointOrder];
        flightPlan.RoutePoints.RemoveAt(pointOrder);
        for (var i = pointOrder; i < flightPlan.RoutePoints.Count; i++)
        {
            flightPlan.RoutePoints[i].Order -= 1;
            await _routePointRepository.SaveAsync(flightPlan.RoutePoints[i]);
        }

        await _flightPlanRepository.SaveAsync(flightPlan);
        
    }

    public async Task RemoveDepartureRunwayFromFlightPlansByRunwayId(Guid runwayId)
    {
        var flightplans = await _flightPlanRepository.GetFlightPlansByDepartureRunwayId(runwayId);
        foreach (var flightplan in flightplans)
        {
            flightplan.DepartureRunway = null;
            flightplan.DepartureRunwayId = null;
            await _flightPlanRepository.SaveAsync(flightplan);
        }
    }

    public async Task RemoveArrivalRunwayFromFlightPlansByRunwayId(Guid runwayId)
    {
        var flightplans = await _flightPlanRepository.GetFlightPlansByArrivalRunwayId(runwayId);
        foreach (var flightplan in flightplans)
        {
            flightplan.ArrivalRunway = null;
            flightplan.ArrivalRunwayId = null;
            await _flightPlanRepository.SaveAsync(flightplan);
        }
    }
}