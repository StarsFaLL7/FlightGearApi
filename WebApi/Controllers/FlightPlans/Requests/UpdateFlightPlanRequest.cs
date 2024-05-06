namespace WebApi.Controllers.FlightPlans.Requests;

public class UpdateFlightPlanRequest
{
    public required string Title { get; set; }
    
    public string? Remarks { get; set; }
    
    public Guid? DepartureRunwayId { get; set; }
    
    public Guid? ArrivalRunwayId { get; set; }
}