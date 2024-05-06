namespace webapi.Controllers.Runways.Responses;

public class FlightFunctionResponse
{
    public required string? Description { get; set; }
    
    public required FunctionPointResponse[] Points { get; set; }
}