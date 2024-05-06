using WebApi.Controllers.Airports.Responses;

namespace WebApi.Controllers.FlightPlans.Responses;

public class RunwayResponse
{
    public required Guid Id { get; set; }
    
    public required string Title { get; set; }
    
    public required bool CanBeDeparture { get; set; }
    
    public required bool CanBeArrival { get; set; }
    
    public required AirportBasicInfoResponse Airport { get; set; }
}