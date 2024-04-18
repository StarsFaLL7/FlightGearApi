using System.ComponentModel.DataAnnotations.Schema;
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
    
    public Guid? NextPointId { get; set; }
    [ForeignKey("NextPointId")]
    public RoutePoint? NextRoutePoint { get; set; }
    
    public Guid? PreviousPointId { get; set; }
    [ForeignKey("NextPointId")]
    public RoutePoint? PreviousRoutePoint { get; set; }
    
    public required Guid FlightPlanId { get; set; }
    [ForeignKey("FlightPlanId")]
    public FlightPlan FlightPlan { get; set; }
    
    public double GetDistanceToIt()
    {
        if (PreviousRoutePoint is null)
        {
            return 0;
        }

        return GeographyHelper.GetDistanceBetweenCoordsInMeters(PreviousRoutePoint.Latitude, PreviousRoutePoint.Longitude,
            Latitude, Longitude);
    }
    
    public double GetDirectionToNextPoint()
    {
        if (NextRoutePoint is null)
        {
            return 0;
        }

        return GeographyHelper.GetDirectionDeg(Latitude, Longitude,
            NextRoutePoint.Latitude, NextRoutePoint.Longitude);
    }

    public double GetRotationDegrees()
    {
        if (NextRoutePoint is null || PreviousRoutePoint is null)
        {
            return 0;
        }

        var prevAngle = GeographyHelper.GetDirectionDeg(PreviousRoutePoint.Latitude, 
            PreviousRoutePoint.Longitude, Latitude, Longitude);
        var curAngle = GeographyHelper.GetDirectionDeg(Latitude, Longitude,
            NextRoutePoint.Latitude, NextRoutePoint.Longitude);
        var angle = Math.Abs(prevAngle - curAngle);
        if (angle > 180)
        {
            angle = 360 - angle;
        }
        return angle;
    }
}