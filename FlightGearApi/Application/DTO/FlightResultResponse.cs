namespace FlightGearApi.Application.DTO;

public class FlightResultResponse
{
    public string Name { get; set; }
    
    public List<PropertyValue> Data { get; set; }
}

public record PropertyValue(int Id, double Value);