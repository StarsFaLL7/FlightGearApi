using System.Reflection;
using Application.Dto;
using Domain.Attributes;
using Domain.Entities;
using Domain.Enums.FlightExportProperty;
using WebApi.Controllers.Airports.Responses;
using webapi.Controllers.Analytics.Responses;
using WebApi.Controllers.FlightPlans.Responses;
using webapi.Controllers.Runways.Responses;
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
                Altitude = point.Altitude,
                Remarks = point.Remarks
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
            Altitude = point.Altitude,
            Remarks = point.Remarks
        }).ToArray();

        return result;
    }
    
    public static AirportResponse ConvertAggregatedAirportToAirportResponse(Airport airport)
    {
        var runways = airport.Runways == null
            ? Array.Empty<RunwayBasicInfoResponse>()
            : airport.Runways.Select(r => new RunwayBasicInfoResponse
            {
                Id = r.Id,
                Title = r.Title,
                CanBeDeparture = r.DepartureFunctionId != null,
                CanBeArrival = r.ArrivalFunctionId != null
            }).ToArray();
        
        var result = new AirportResponse
        {
            Id = airport.Id,
            Title = airport.Title,
            Code = airport.Code,
            City = airport.City,
            Runways = runways
        };

        return result;
    }

    public static FlightPropertyReadsResultResponse[] ConvertPropertyShotsToPropertiesResponseArray(FlightPropertiesShot[] shots)
    {
        var propertyInfos = typeof(FlightPropertiesShot).GetProperties()
            .Where(p => Attribute.IsDefined(p, typeof(FlightPropertyInfoAttribute)))
            .ToArray();
        var shotsOrdered = shots.OrderBy(s => s.Order).ToArray();
        var result = new List<FlightPropertyReadsResultResponse>();
        foreach (var propertyInfo in propertyInfos)
        {
            var attribute = propertyInfo.GetCustomAttribute<FlightPropertyInfoAttribute>();
            var dataList = new List<PropertyIdValuePairResponse>();
            var resp = new FlightPropertyReadsResultResponse
            {
                Name = attribute.ExportPropertyEnum.GetRussianVariant(),
                Data = Array.Empty<PropertyIdValuePairResponse>()
            };
            var id = 0;
            foreach (var shot in shotsOrdered)
            {
                var value = (double)propertyInfo.GetValue(shot);
                dataList.Add(new PropertyIdValuePairResponse
                {
                    Id = id,
                    Value = value
                });
                id++;
            }

            resp.Data = dataList.ToArray();
            result.Add(resp);
        }

        return result.ToArray();
    }

    public static RunwayFullResponse ConvertAggregatedRunwayToFullResponse(AirportRunway runway)
    {
        var departureFunc = runway.DepartureFunction == null
            ? null
            : new FlightFunctionResponse
            {
                Description = runway.DepartureFunction.Description,
                Points = runway.DepartureFunction.FunctionPoints
                    .OrderBy(p => p.Order)
                    .Select(p => new FunctionPointResponse
                    {
                        Order = p.Order,
                        Longitude = p.Longitude,
                        Latitude = p.Latitude,
                        Altitude = p.Altitude,
                        Speed = p.Speed,
                        Remarks = p.Remarks
                    }).ToArray()
            };
        var arrivalFunc = runway.ArrivalFunction == null
            ? null
            : new FlightFunctionResponse
            {
                Description = runway.ArrivalFunction.Description,
                Points = runway.ArrivalFunction.FunctionPoints
                    .OrderBy(p => p.Order)
                    .Select(p => new FunctionPointResponse
                    {
                        Order = p.Order,
                        Longitude = p.Longitude,
                        Latitude = p.Latitude,
                        Altitude = p.Altitude,
                        Speed = p.Speed,
                        Remarks = p.Remarks
                    }).ToArray()
            };
        var result = new RunwayFullResponse
        {
            Id = runway.Id,
            Title = runway.Title,
            DepartureFunction = departureFunc,
            ArrivalFunction = arrivalFunc,
            Airport = new AirportBasicInfoResponse
            {
                Id = runway.Airport.Id,
                Title = runway.Airport.Title,
                Code = runway.Airport.Code,
                City = runway.Airport.City
            }
        };
        return result;
    }
}