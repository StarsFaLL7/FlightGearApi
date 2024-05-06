namespace webapi.Controllers.Runways.Responses;

public class FunctionPointResponse
{
    public required int Order { get; set; }
    
    public required double Longitude { get; set; }
    
    public required double Latitude { get; set; }
    
    public required double Altitude { get; set; }
    
    public required double Speed { get; set; }
    
    public required string? Remarks { get; set; }
}