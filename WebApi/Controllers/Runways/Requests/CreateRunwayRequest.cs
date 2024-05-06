namespace webapi.Controllers.Runways.Requests;

public class CreateRunwayRequest
{
    public required string Title { get; set; }

    public required Guid AirportId { get; set; }
}