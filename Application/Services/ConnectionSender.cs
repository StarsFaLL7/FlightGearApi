using Application.Interfaces;
using Domain.Enums.FlightUtilityProperty;

namespace Application.Services;

internal class ConnectionSender : IConnectionSender
{
    public ConnectionSender()
    {
    }

    public async Task SendParametersAsync(Dictionary<FlightUtilityProperty, double> propertiesToChange)
    {
        throw new NotImplementedException();
    }

    public byte[] ConvertDoublesToBigEndianBytes(double[] numbers)
    {
        var totalBytes = sizeof(double) * numbers.Length;
        var data = new byte[totalBytes];

        for (var i = 0; i < numbers.Length; i++)
        {
            var doubleBytes = BitConverter.GetBytes(numbers[i]);
            
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(doubleBytes);
            }
            Array.Copy(doubleBytes, 0, data, i * sizeof(double), sizeof(double));
        }
        return data;
    }
}