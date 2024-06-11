using System.Text;
using Application.Interfaces;
using Application.Interfaces.Entities;
using Domain.Entities;
using Domain.Utility;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Services;

public class XmlFileManager : IXmlFileManager
{
    private readonly IServiceProvider _serviceProvider;
    private readonly string _pathToExportXmlFile;
    private readonly string _pathToRouteXmlFile;
    
    public XmlFileManager(IConfiguration configuration, IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
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

    public async Task CreateOrUpdateExportXmlFileAsync()
    {
        if (!File.Exists(_pathToExportXmlFile))
        {
            File.Copy("fg-export.xml", _pathToExportXmlFile);
        }
    }

    public async Task CreateOrUpdateRouteManagerXmlFileAsync(FlightPlan flightPlan)
    {
        var content = await GetRouteXmlContent(flightPlan);
        try
        {
            using (var fs = new FileStream(_pathToRouteXmlFile, FileMode.Create))
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
        /*if (startFromAirport)
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
        }*/

        builder.Append("\t<route>\n");
        var wpindex = 0;
        var scopeFactory = _serviceProvider.GetRequiredService<IServiceScopeFactory>();
        using var scope = scopeFactory.CreateScope();
        var runwayService = scope.ServiceProvider.GetRequiredService<IRunwayService>();
        if (startFromAirport)
        {
            var runway = await runwayService.GetAggregatedRunwayByIdAsync(flightPlan.DepartureRunway.Id);
            foreach (var point in runway.DepartureFunction.FunctionPoints.OrderBy(p => p.Order))
            {
                builder.Append($"\t\t<wp n=\"{wpindex}\">\n" +
                               "\t\t\t<type type=\"string\">basic</type>\n" +
                               "\t\t\t<alt-restrict type=\"string\">at</alt-restrict>\n" +
                               $"\t\t\t<altitude-ft type=\"double\">{point.Altitude * 3.282}</altitude-ft>\n" +
                               $"\t\t\t<knots type=\"int\">{point.Speed}</knots>\n" +
                               $"\t\t\t<ident type=\"string\">STARTF-{wpindex}</ident>\n" +
                               $"\t\t\t<lon type=\"double\">{point.Longitude}</lon>\n" +
                               $"\t\t\t<lat type=\"double\">{point.Latitude}</lat>\n" +
                               "\t\t</wp>\n");
                wpindex++;
            }
        }

        var normalSpeed = 600; 
        var index = 0;
        foreach (var point in flightPlan.RoutePoints)
        {
            /*if (index > 0 && flightPlan.RoutePoints.Count > index + 1)
            {
                var nextPoint = flightPlan.RoutePoints[index + 1];
                var prevPoint = flightPlan.RoutePoints[index - 1];
                var rotationAgle = point.GetRotationDegrees(prevPoint, nextPoint);
                if (rotationAgle > 80)
                {
                    var dist = point.GetDistanceToIt(prevPoint);
                    var direction = GeographyHelper.GetDirectionDeg(point.Latitude, point.Longitude, 
                        prevPoint.Latitude, prevPoint.Longitude);
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
            }*/
            var altitude = point.Altitude * 3.282;
            builder.Append($"\t\t<wp n=\"{wpindex}\">\n" +
                           "\t\t\t<type type=\"string\">basic</type>\n" +
                           "\t\t\t<alt-restrict type=\"string\">at</alt-restrict>\n" +
                           $"\t\t\t<altitude-ft type=\"double\">{altitude}</altitude-ft>\n" +
                           $"\t\t\t<knots type=\"int\">{normalSpeed}</knots>\n" +
                           $"\t\t\t<ident type=\"string\">WP-USER-{wpindex}</ident>\n" +
                           $"\t\t\t<lon type=\"double\">{point.Longitude}</lon>\n" +
                           $"\t\t\t<lat type=\"double\">{point.Latitude}</lat>\n" +
                           "\t\t</wp>\n");
            wpindex++;
            index++;
        }
        
        if (endOnAirport)
        {
            var runway = await runwayService.GetAggregatedRunwayByIdAsync(flightPlan.ArrivalRunway.Id);
            foreach (var point in runway.ArrivalFunction.FunctionPoints.OrderBy(p => p.Order))
            {
                builder.Append($"\t\t<wp n=\"{wpindex}\">\n" +
                               "\t\t\t<type type=\"string\">basic</type>\n" +
                               "\t\t\t<alt-restrict type=\"string\">at</alt-restrict>\n" +
                               $"\t\t\t<altitude-ft type=\"double\">{point.Altitude * 3.282}</altitude-ft>\n" +
                               $"\t\t\t<knots type=\"int\">{point.Speed}</knots>\n" +
                               $"\t\t\t<ident type=\"string\">ENDF-{wpindex}</ident>\n" +
                               $"\t\t\t<lon type=\"double\">{point.Longitude}</lon>\n" +
                               $"\t\t\t<lat type=\"double\">{point.Latitude}</lat>\n" +
                               "\t\t</wp>\n");
                wpindex++;
            }
        }
        builder.Append("\t</route>\n");
        builder.Append("</PropertyList>\n");
        return builder.ToString();
    }
}