namespace WebApi.Controllers.Airports.Responses;

public class GetAllAirportsResponse
{
    public required AirportBasicInfoResponse[] Airports { get; set; }
}