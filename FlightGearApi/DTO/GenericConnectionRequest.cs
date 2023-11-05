using FlightGearApi.Enums;

namespace FlightGearApi.DTO;

public class GenericConnectionRequest
{
    public IoType IoType { get; set; }
    
    public int Port { get; set; }
    
    public int RefreshesPerSecond { get; set; }
    
    public string? ProtocolFileName { get; set; }
    
    public string Address { get; set; } = "127.0.0.1";
}