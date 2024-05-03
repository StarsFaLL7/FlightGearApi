namespace WebApi.Controllers.FlightPlans.Responses;

public class GetAllFlightPlansResponse
{
    public required BasicFlightPlanInfo[] FlightPlans { get; set; }
}