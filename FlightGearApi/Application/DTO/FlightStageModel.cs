namespace FlightGearApi.Application.DTO;

public class FlightStageModel
{
    public int Index { get; set; }
    public double Altitude { get; set; }
    public double Heading { get; set; }
    public double Speed { get; set; }

    public FlightStageModel(double altitude, double heading, double speed)
    {
        Altitude = altitude;
        Heading = heading;
        Speed = speed;
    }
}