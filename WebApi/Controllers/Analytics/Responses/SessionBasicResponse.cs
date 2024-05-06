namespace WebApi.Controllers.Responses;

public class SessionBasicResponse
{
    public required Guid Id { get; set; }
    
    public required string Title { get; set; }
    
    public required int PropertiesReadsPerSec { get; set; }
    
    public required DateTime DateTimeStart { get; set; }
    
    public required int DurationSec { get; set; }

    public required  DateTime DateTimeEnd { get; set; }
}