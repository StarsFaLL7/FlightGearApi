namespace WebApi.Controllers.FlightPlans.Responses;

public class GetFlightPlanPointsResponse
{
    public required RoutePointResponse[] RoutePoints { get; set; }
}