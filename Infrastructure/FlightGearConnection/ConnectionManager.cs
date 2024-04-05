using Application.Interfaces.Connection;
using Domain.Entities;
using Domain.Enums.FlightUtilityProperty;

namespace Infrastructure.FlightGearConnection;

internal class ConnectionManager : IConnectionManager
{
    
    public ConnectionManager()
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

    public async Task SendParametersAsync(Dictionary<FlightUtilityProperty, double> propertiesToChange)
    {
        throw new NotImplementedException();
    }

    public byte[] ConvertDoublesToBigEndianBytes(double[] numbers)
    {
        throw new NotImplementedException();
    }
}