using Application.Interfaces;
using Domain.Entities;

namespace Application.Services;

public class XmlFileManager : IXmlFileManager
{
    public XmlFileManager()
    {
    }

    public async Task CreateOrUpdateExportXmlFileAsync(int readsPerSecond)
    {
        throw new NotImplementedException();
    }

    public async Task CreateOrUpdateRouteManagerXmlFileAsync(FlightPlan flightPlan)
    {
        throw new NotImplementedException();
    }
}