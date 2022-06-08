using System.Collections.Concurrent;
using Ozon256.WeatherSensors.Contracts;

namespace Ozon256.WeatherSensors.DataProcessorClient.SensorsDataStorage;

public class SensorsDataStorage : ISensorsDataStorage
{
    private readonly ConcurrentDictionary<Guid, List<ISensorData>> _sensorsData = new();
    
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

    public void RemoveSensor(Guid guid)
    {
        _sensorsData.TryRemove(guid, out var sensorsData);
    }

    public void AddSensor(Guid guid)
    {
        _sensorsData.TryAdd(guid, new List<ISensorData>());
    }
}