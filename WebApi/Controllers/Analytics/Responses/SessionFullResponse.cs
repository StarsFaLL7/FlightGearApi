using WebApi.Controllers.Responses;

namespace webapi.Controllers.Analytics.Responses;

public class SessionFullResponse : SessionBasicResponse
{
    public required FlightPropertyReadsResultResponse[] Properties { get; set; }
}