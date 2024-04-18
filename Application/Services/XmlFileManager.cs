using System.Text;
using Application.Interfaces;
using Domain.Entities;
using Domain.Utility;
using Microsoft.Extensions.Configuration;

namespace Application.Services;

public class XmlFileManager : IXmlFileManager
{
    private readonly string _pathToExportXmlFile;
    private readonly string _pathToRouteXmlFile;

    
    public XmlFileManager(IConfiguration configuration)
    {
        var fgSection = configuration.GetSection("FlightGearSettings");
        var routeManagerSection = fgSection.GetSection("RouteManager");
        _pathToRouteXmlFile = Path.Combine(routeManagerSection.GetValue<string>("RoutePlanPath"), 
            routeManagerSection.GetValue<string>("RoutePlanFileName"));
        var foldersSection = fgSection.GetSection("Folders");
        var exportSection = fgSection.GetSection("Export");
        _pathToExportXmlFile = Path.Combine(foldersSection.GetValue<string>("MainFolder"), 
            foldersSection.GetValue<string>("ProtocolSubPath"),
            exportSection.GetValue<string>("XmlExportFilename") + ".xml");
    }

    public async Task CreateOrUpdateExportXmlFileAsync(int readsPerSecond)
    {
        throw new NotImplementedException();
    }

    public async Task CreateOrUpdateRouteManagerXmlFileAsync(FlightPlan flightPlan)
    {
        var content = await GetRouteXmlContent(flightPlan);
        try
        {
            using (var fs = new FileStream(_pathToRouteXmlFile, FileMode.OpenOrCreate))
            {
                var bytes = Encoding.UTF8.GetBytes(content);
                await fs.WriteAsync(bytes);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    private async Task<string> GetRouteXmlContent(FlightPlan flightPlan)
    {
        var builder = new StringBuilder();
        var startFromAirport = flightPlan.DepartureRunway != null;
        var endOnAirport = flightPlan.ArrivalRunway != null;
        builder.Append("<?xml version=\"1.0\"?>\n<PropertyList>\n\t<version type=\"int\">2</version>\n");
        if (startFromAirport)
        {
            builder.Append("\t<departure>\n" +
                           $"\t\t<airport type=\"string\">{flightPlan.DepartureRunway.Airport.Code}</airport>\n" +
                           $"\t\t<runway type=\"string\">{flightPlan.DepartureRunway.Title}</runway>" +
                           "\t</departure>\n");
        }
        if (endOnAirport)
        {
            builder.Append("\t<destination>\n" +
                           $"\t\t<airport type=\"string\">{flightPlan.ArrivalRunway.Airport.Code}</airport>\n" +
                           $"\t\t<runway type=\"string\">{flightPlan.ArrivalRunway.Title}</runway>" +
                           "\t</destination>\n");
        }

        builder.Append("\t<route>\n");
        var wpindex = 1;
        if (startFromAirport)
        {
            foreach (var point in flightPlan.DepartureRunway.DepartureFunction.FunctionPoints.OrderBy(p => p.Order))
            {
                builder.Append($"\t\t<wp n=\"{wpindex}\">\n" +
                               "\t\t\t<type type=\"string\">basic</type>\n" +
                               "\t\t\t<alt-restrict type=\"string\">at</alt-restrict>\n" +
                               $"\t\t\t<altitude-ft type=\"double\">{point.Altitude}</altitude-ft>\n" +
                               $"\t\t\t<knots type=\"int\">{point.Speed}</knots>\n" +
                               $"\t\t\t<ident type=\"string\">STARTF-{wpindex}</ident>\n" +
                               $"\t\t\t<lon type=\"double\">{point.Longitude}</lon>\n" +
                               $"\t\t\t<lat type=\"double\">{point.Latitude}</lat>\n" +
                               "\t\t</wp>\n");
            }
        }

        var normalSpeed = 600;
        foreach (var point in flightPlan.RoutePoints.OrderBy(p => p.Order))
        {
            var rotationAgle = point.GetRotationDegrees();
            if (rotationAgle > 80)
            {
                var dist = point.GetDistanceToIt();
                var direction = GeographyHelper.GetDirectionDeg(point.Latitude, point.Longitude, 
                    point.PreviousRoutePoint.Latitude, point.PreviousRoutePoint.Longitude);
                var utilityPoint = GeographyHelper.MoveGeoPoint(point.Latitude, point.Longitude, dist * 0.3, direction);
                var speed = Math.Round(Math.Max(120, 600 - (rotationAgle / 30) * 100));
                builder.Append($"\t\t<wp n=\"{wpindex}\">\n" +
                               "\t\t\t<type type=\"string\">basic</type>\n" +
                               "\t\t\t<alt-restrict type=\"string\">at</alt-restrict>\n" +
                               $"\t\t\t<altitude-ft type=\"double\">{point.Altitude}</altitude-ft>\n" +
                               $"\t\t\t<knots type=\"int\">{speed}</knots>\n" +
                               $"\t\t\t<ident type=\"string\">WP-UTILITY-{wpindex}</ident>\n" +
                               $"\t\t\t<lon type=\"double\">{utilityPoint.Longitude}</lon>\n" +
                               $"\t\t\t<lat type=\"double\">{utilityPoint.Latitude}</lat>\n" +
                               "\t\t</wp>\n");
                wpindex++;
            }
            
            builder.Append($"\t\t<wp n=\"{wpindex}\">\n" +
                           "\t\t\t<type type=\"string\">basic</type>\n" +
                           "\t\t\t<alt-restrict type=\"string\">at</alt-restrict>\n" +
                           $"\t\t\t<altitude-ft type=\"double\">{point.Altitude}</altitude-ft>\n" +
                           $"\t\t\t<knots type=\"int\">{normalSpeed}</knots>\n" +
                           $"\t\t\t<ident type=\"string\">WP-USER-{wpindex}</ident>\n" +
                           $"\t\t\t<lon type=\"double\">{point.Longitude}</lon>\n" +
                           $"\t\t\t<lat type=\"double\">{point.Latitude}</lat>\n" +
                           "\t\t</wp>\n");
            wpindex++;
        }
        
        if (endOnAirport)
        {
            foreach (var point in flightPlan.ArrivalRunway.ArrivalFunction.FunctionPoints.OrderBy(p => p.Order))
            {
                builder.Append($"\t\t<wp n=\"{wpindex}\">\n" +
                               "\t\t\t<type type=\"string\">basic</type>\n" +
                               "\t\t\t<alt-restrict type=\"string\">at</alt-restrict>\n" +
                               $"\t\t\t<altitude-ft type=\"double\">{point.Altitude}</altitude-ft>\n" +
                               $"\t\t\t<knots type=\"int\">{point.Speed}</knots>\n" +
                               $"\t\t\t<ident type=\"string\">ENDF-{wpindex}</ident>\n" +
                               $"\t\t\t<lon type=\"double\">{point.Longitude}</lon>\n" +
                               $"\t\t\t<lat type=\"double\">{point.Latitude}</lat>\n" +
                               "\t\t</wp>\n");
            }
        }
        builder.Append("\t</route>\n");
        builder.Append("</PropertyList>\n");
        return builder.ToString();
    }
}