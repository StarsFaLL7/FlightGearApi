using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Domain.Base;
using Domain.Utility;

namespace Domain.Entities;

public class RoutePoint : BaseEntityWithKey<Guid>
{
    public required int Order { get; set; }
    
    public required double Longitude { get; set; }
    
    public required double Latitude { get; set; }
    
    public required double Altitude { get; set; }
    
    public string? Remarks { get; set; }
    

    
    public required Guid FlightPlanId { get; set; }
    [ForeignKey("FlightPlanId")]
    [JsonIgnore]
    public FlightPlan FlightPlan { get; set; }
    
    public double GetDistanceToIt(RoutePoint? previousPoint)
    {
        if (previousPoint is null)
        {
            return 0;
        }

        return GeographyHelper.GetDistanceBetweenCoordsInMeters(previousPoint.Latitude, previousPoint.Longitude,
            Latitude, Longitude);
    }
    
    public double GetDirectionToNextPoint(RoutePoint? nextPoint)
    {
        if (nextPoint is null)
        {
            return 0;
        }

        return GeographyHelper.GetDirectionDeg(Latitude, Longitude,
            nextPoint.Latitude, nextPoint.Longitude);
    }

    public double GetRotationDegrees(RoutePoint? previousPoint, RoutePoint? nextPoint)
    {
        if (previousPoint is null || nextPoint is null)
        {
            return 0;
        }

        var prevAngle = GeographyHelper.GetDirectionDeg(previousPoint.Latitude, 
            previousPoint.Longitude, Latitude, Longitude);
        var curAngle = GeographyHelper.GetDirectionDeg(Latitude, Longitude,
            nextPoint.Latitude, nextPoint.Longitude);
        var angle = Math.Abs(prevAngle - curAngle);
        if (angle > 180)
        {
            angle = 360 - angle;
        }
        return angle;
    }
}