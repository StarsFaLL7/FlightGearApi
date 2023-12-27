namespace FlightGearApi.Application.DTO;

public class FlightStageDto
{
    public int Index { get; set; }
    public double Altitude { get; set; }
    public double Heading { get; set; }
    public double Speed { get; set; }

    public FlightStageDto(double altitude, double heading, double speed)
    {
        Altitude = altitude;
        Heading = heading;
        Speed = speed;
    }
}