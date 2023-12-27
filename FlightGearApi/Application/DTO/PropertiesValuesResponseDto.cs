namespace FlightGearApi.Application.DTO;

public class PropertiesValuesResponseDto
{
    public string Name { get; set; }
    public List<PropertyValueDto> Data { get; set; }
}