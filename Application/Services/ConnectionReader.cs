using Application.Interfaces;
using Domain.Entities;
using Domain.Enums.FlightUtilityProperty;

namespace Application.Services;

internal class ConnectionReader : IConnectionReader
{
    public ConnectionReader()
    {
    }

    public async Task<Dictionary<string, double>> GetCurrentUtilityValuesAsync(params FlightUtilityProperty[] properties)
    {
        throw new NotImplementedException();
    }

    public async Task<FlightPropertiesShot> GetCurrentValuesAsync()
    {
        throw new NotImplementedException();
    }
}