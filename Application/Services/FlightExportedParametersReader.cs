using Application.Interfaces;
using Domain.Entities;

namespace Application.Services;

public class FlightExportedParametersReader : IFlightExportedParametersReader
{
    public FlightExportedParametersReader()
    {
    }

    public async Task<FlightPropertiesShot[]> GetExportedPropertiesAsync(string pathToExportFile)
    {
        throw new NotImplementedException();
    }
}