namespace WebApi.Controllers.FlightPlans.Responses;

public class RoutePointResponse
{
    public required Guid Id { get; set; }
    
    public required int Order { get; set; }
    
    public required bool IsEditable { get; set; }
    
    public required double Longitude { get; set; }
    
    public required double Latitude { get; set; }
    
    public required double Altitude { get; set; }
    
    public string? Remarks { get; set; }
}