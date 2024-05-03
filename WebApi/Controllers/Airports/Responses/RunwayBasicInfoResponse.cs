namespace WebApi.Controllers.Airports.Responses;

public class RunwayBasicInfoResponse
{
    public required Guid Id { get; set; }
    
    public required string Title { get; set; }
    
    public required bool CanBeDeparture { get; set; }

    public required bool CanBeArrival { get; set; }
}