using WebApi.Controllers.Airports.Responses;

namespace webapi.Controllers.Runways.Responses;

public class RunwayFullResponse
{
    public required Guid Id { get; set; }
    
    public required string Title { get; set; }
    
    public required FlightFunctionResponse? DepartureFunction { get; set; }
    
    public required FlightFunctionResponse? ArrivalFunction { get; set; }
    
    public required AirportBasicInfoResponse Airport { get; set; }
}