namespace WebApi.Controllers.Airports.Requests;

public class UpdateAirportRequest
{
    public required string Title { get; set; }
    
    public required string Code { get; set; }
    
    public required string City { get; set; }
}