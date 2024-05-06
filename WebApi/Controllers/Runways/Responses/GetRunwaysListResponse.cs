using WebApi.Controllers.Airports.Responses;
using WebApi.Controllers.FlightPlans.Responses;

namespace webapi.Controllers.Runways.Responses;

public class GetRunwaysListResponse
{
    public required RunwayBasicInfoResponse[] Runways { get; set; }
}