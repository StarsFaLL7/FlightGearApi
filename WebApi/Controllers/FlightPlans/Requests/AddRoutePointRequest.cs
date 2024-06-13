namespace WebApi.Controllers.FlightPlans.Requests;

public class AddRoutePointRequest
{
   /*  public required int Order { get; set; } */
    
    public required double Longitude { get; set; }
    
    public required double Latitude { get; set; }
    
    public required double Altitude { get; set; }
    
    public string? Remarks { get; set; }
}