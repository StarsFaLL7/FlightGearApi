using WebApi.Controllers.Responses;

namespace webapi.Controllers.Analytics.Responses;

public class GetAllSessionsResponse
{
    public required SessionBasicResponse[] Sessions { get; set; }
}