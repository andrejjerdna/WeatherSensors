using System.Collections.Concurrent;
using Microsoft.Extensions.Options;
using Ozon256.WeatherSensors.Contracts;
using Ozon256.WeatherSensors.SensorsEmulatorService.Options;

namespace Ozon256.WeatherSensors.SensorsEmulatorService.Models;

public class SensorsPool : ISensorsPool
{
    private readonly ConcurrentDictionary<Guid, ISensor> _sensors = new();

    public Task<IEnumerable<Guid>> GetAllSensorsGuids()
    {
        return Task.FromResult(_sensors.Select(s => s.Key));
    }

    public Task<IEnumerable<ISensorData>> GetSensorsData() => Task.FromResult(_sensors.Values.Select(s => s.GetData()));
    
    public Task<ISensor?> GetSensorsByGuid(Guid guid)
    {
        if (_sensors.TryGetValue(guid, out var sensor))
            return Task.FromResult(sensor);

        return Task.FromResult(default(ISensor));
    }

    public Task<bool> DeleteSensorsByGuid(Guid guid)
    {
        return Task.FromResult(_sensors.TryRemove(guid, out var sensor));
    }
    
    public Task<bool> AddSensors(ISensor sensor)
    {
        return Task.FromResult(_sensors.TryAdd(sensor.Guid, sensor));
    }
}