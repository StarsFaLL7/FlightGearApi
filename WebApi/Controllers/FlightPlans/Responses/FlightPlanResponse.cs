namespace WebApi.Controllers.FlightPlans.Responses;

public class FlightPlanResponse
{
    public required Guid Id { get; set; }
    
    public required string Title { get; set; }
    
    public string? Remarks { get; set; }
    
    public RunwayResponse? DepartureRunway { get; set; }
    
    public RunwayResponse? ArrivalRunway { get; set; }
    
    public required RoutePointResponse[] RoutePoints { get; set; }
}