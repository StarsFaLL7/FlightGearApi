using FlightGearApi.records;

namespace FlightGearApi.FlightGearCore;

public class ConnectionListener
{
    public async void StartNewListenAsync(GenericConnectionInfo connectionInfo, string name)
    {
        // TODO Начало прослушивания с сессией name
    }

    public async void StopListenAsync(string name)
    {
        // TODO Конец сессии прослушивания name
    }

    public async Task<Dictionary<string, object>> GetCurrentValues()
    {
        // TODO дождаться получения значений, распарсить и вернуть их
        return new();
    }

    public void SaveResultsToFile(string filename)
    {
        // TODO 
    }
}