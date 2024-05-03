namespace webapi.Controllers.Flight.Responses;

public class FlightPositionResponse
{
    public required string Status { get; set; }
    
    public required double Longitude { get; set; }
    
    public required double Latitude { get; set; }
    
    public required double AltitudeAgl { get; set; } // Истинная высота (над уровнем земли)
    
    public required double Altitude { get; set; }
    
    public required double Roll { get; set; }
    
    public required double Pitch { get; set; }
    
    public required double Heading { get; set; }
    
    public required double IndicatedSpeed { get; set; } // Приборная скорость
    
    public required double Airspeed { get; set; }
}