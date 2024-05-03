namespace WebApi.Controllers.FlightPlans.Responses;

public class BasicFlightPlanInfo
{
    public required Guid Id { get; set; }
    
    public required string Title { get; set; }
}