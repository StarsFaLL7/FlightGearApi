namespace WebApi.Controllers.Airports.Responses;

public class AirportResponse
{
    public required Guid Id { get; set; }
    
    public required string Title { get; set; }
    
    public required string Code { get; set; }
    
    public required string City { get; set; }
    
    public required RunwayBasicInfoResponse[] Runways { get; set; }
}