using System.Collections.Concurrent;
using Ozon256.WeatherSensors.Contracts;

namespace Ozon256.WeatherSensors.DataProcessorClient.SensorsDataStorage;

public class SensorsDataStorage : ISensorsDataStorage
{
    private readonly Queue<Guid> _scriptionSensors = new();
    private readonly Queue<Guid> _ubscriptionSensors = new();
    
    private readonly ConcurrentDictionary<Guid, List<ISensorData>> _sensorsData = new();
    
    private readonly ConcurrentDictionary<Guid, double> _averageTemperature = new();
    private readonly ConcurrentDictionary<Guid, double> _averageHumidity = new();
    private readonly ConcurrentDictionary<Guid, double> _averagePpm = new();

    public Queue<Guid> SubscriptionSensors => _scriptionSensors;
    public Queue<Guid> UnSubscriptionSensors => _ubscriptionSensors;

    public Task<bool> AddSubscriptionSensor(Guid guid)
    {
        _scriptionSensors.Enqueue(guid);
        
        return Task.FromResult(true);
    }

    public Task<bool> AddUnsubscriptionSensor(Guid guid)
    {
        _ubscriptionSensors.Enqueue(guid);
        
        return Task.FromResult(true);
    }

    public void Add(ISensorData sensorData)
    {
        if (_sensorsData.TryGetValue(sensorData.SensorGuid, out var sensorsData))
        {
            sensorsData.Add(sensorData);
        }
        else
        {
            _sensorsData.TryAdd(sensorData.SensorGuid, new List<ISensorData>{ sensorData });
        }
    }

    public async Task<IEnumerable<ISensorData>?> GetSensorDataByGuid(Guid guid)
    {
        return await Task.Run (() => _sensorsData.TryGetValue(guid, out var sensorsData) ? sensorsData : null);
    }
}