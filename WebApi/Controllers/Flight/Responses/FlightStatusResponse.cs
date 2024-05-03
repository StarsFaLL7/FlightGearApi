namespace webapi.Controllers.Flight.Responses;

public class FlightStatusResponse
{
    public required string Status { get; set; }
    
    public required bool IsRunning { get; set; }
    
    public required int LastReachedPointOrder { get; set; }
    
    public required int PercentCompleted { get; set; }
}