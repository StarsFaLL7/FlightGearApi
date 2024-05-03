using WebApi.Controllers.Base;

namespace WebApi.Controllers.FlightPlans.Responses;

public class FlightPlanWithStatusResponse : BasicStatusResponse
{
    public required FlightPlanResponse FlightPlan { get; set; }
}