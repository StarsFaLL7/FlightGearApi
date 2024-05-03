using Application.Dto;
using Domain.Entities;
using WebApi.Controllers.Airports.Responses;
using WebApi.Controllers.FlightPlans.Responses;
using RunwayResponse = WebApi.Controllers.FlightPlans.Responses.RunwayResponse;

namespace webapi.Controllers;

public static class DtoConverter
{
    public static GetAllFlightPlansResponse ConvertFlightPlansArrayToBasicInfoResponse(FlightPlan[] flightPlans)
    {
        var result = new GetAllFlightPlansResponse
        {
            FlightPlans = flightPlans.Select(plan => new BasicFlightPlanInfo
            {
                Id = plan.Id,
                Title = plan.Title
            }).ToArray()
        };
        return result;
    }
    
    public static FlightPlanResponse ConvertAggregatedFlightPlanToResponse(FlightPlanAggregated flightPlanAggregated)
    {
        var result = new FlightPlanResponse
        {
            Id = flightPlanAggregated.Id,
            Title = flightPlanAggregated.Title,
            Remarks = flightPlanAggregated.Remarks,
            DepartureRunway = null,
            ArrivalRunway = null,
            RoutePoints = flightPlanAggregated.RoutePoints.Select(point => new RoutePointResponse
            {
                Id = point.Id,
                Order = point.Order,
                IsEditable = point.IsEditable,
                Longitude = point.Longitude,
                Latitude = point.Latitude,
                Altitude = point.Altitude
            }).ToArray()
        };
        if (flightPlanAggregated.DepartureRunway != null)
        {
            result.DepartureRunway = new RunwayResponse
            {
                Id = flightPlanAggregated.DepartureRunway.Id,
                Title = flightPlanAggregated.DepartureRunway.Title,
                CanBeDeparture = flightPlanAggregated.DepartureRunway.DepartureFunction != null,
                CanBeArrival = flightPlanAggregated.DepartureRunway.ArrivalFunction != null,
                Airport = new AirportBasicInfoResponse
                {
                    Id = flightPlanAggregated.DepartureRunway.Airport.Id,
                    Title = flightPlanAggregated.DepartureRunway.Airport.Title,
                    Code = flightPlanAggregated.DepartureRunway.Airport.Code,
                    City = flightPlanAggregated.DepartureRunway.Airport.City
                }
            };
        }
        if (flightPlanAggregated.ArrivalRunway != null)
        {
            result.ArrivalRunway = new RunwayResponse
            {
                Id = flightPlanAggregated.ArrivalRunway.Id,
                Title = flightPlanAggregated.ArrivalRunway.Title,
                CanBeDeparture = flightPlanAggregated.ArrivalRunway.DepartureFunction != null,
                CanBeArrival = flightPlanAggregated.ArrivalRunway.ArrivalFunction != null,
                Airport = new AirportBasicInfoResponse
                {
                    Id = flightPlanAggregated.ArrivalRunway.Airport.Id,
                    Title = flightPlanAggregated.ArrivalRunway.Airport.Title,
                    Code = flightPlanAggregated.ArrivalRunway.Airport.Code,
                    City = flightPlanAggregated.ArrivalRunway.Airport.City
                }
            };
        }

        return result;
    }
    
    public static RoutePointResponse[] ConvertAggregatedFlightPlanToRoutePointsResponseArray(FlightPlanAggregated flightPlanAggregated)
    {
        var result = flightPlanAggregated.RoutePoints
            .Select(point => new RoutePointResponse
        {
            Id = point.Id,
            Order = point.Order,
            IsEditable = point.IsEditable,
            Longitude = point.Longitude,
            Latitude = point.Latitude,
            Altitude = point.Altitude
        }).ToArray();

        return result;
    }
    
    public static AirportResponse ConvertAggregatedAirportToAirportResponse(Airport airport)
    {
        var result = new AirportResponse
        {
            Id = airport.Id,
            Title = airport.Title,
            Code = airport.Code,
            City = airport.City,
            Runways = airport.Runways.Select(r => new RunwayBasicInfoResponse
            {
                Id = r.Id,
                Title = r.Title,
                CanBeDeparture = r.DepartureFunctionId != null,
                CanBeArrival = r.ArrivalFunctionId != null
            }).ToArray()
        };

        return result;
    }
}