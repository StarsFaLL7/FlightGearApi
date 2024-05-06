namespace webapi.Controllers.Flight.Requests;

public class LaunchSimulationRequest
{
    public required string Title { get; set; }
    
    public required int ReadsPerSecond { get; set; }
    
    public required Guid FlightPlanId { get; set; }
}