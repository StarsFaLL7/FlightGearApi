namespace WebApi.Controllers.FlightPlans.Requests;

public class UpdateRoutePointRequest
{
    public required double Longitude { get; set; }
    
    public required double Latitude { get; set; }
    
    public required double Altitude { get; set; }
    
    public string? Remarks { get; set; }
}