namespace webapi.Controllers.Analytics.Responses;

public class FlightPropertyReadsResultResponse
{
    public required string Name { get; set; }
    
    public required PropertyIdValuePairResponse[] Data { get; set; }
}