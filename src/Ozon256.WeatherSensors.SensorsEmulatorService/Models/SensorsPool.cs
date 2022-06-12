using System.Collections.Concurrent;
using Microsoft.Extensions.Options;
using Ozon256.WeatherSensors.Contracts;
using Ozon256.WeatherSensors.Core.Models;
using Ozon256.WeatherSensors.SensorsEmulatorService.Options;

namespace Ozon256.WeatherSensors.SensorsEmulatorService.Models;

public class SensorsPool : ISensorsPool
{
    private readonly ConcurrentDictionary<Guid, ISensor> _sensors = new();
    private readonly ConcurrentDictionary<Guid, DateTime> _sensorsSu = new();
   
    public SensorsPool()
    {
        GetTestData(2);
    }

    public Task<IEnumerable<Guid>> GetAllSensorsGuids()
    {
        return Task.FromResult(_sensors.Select(s => s.Key));
    }

    public Task<IEnumerable<ISensorData>> GetSensorsData()
    {
        return Task.FromResult(_sensors.Values.Where(s => _sensorsSu.ContainsKey(s.Guid)).Select(s => s.GetData()));
    }

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
    
    public async Task<ISensor?> AddNewSensor(SensorType sensorType)
    {
        ISensor result;
        
        if (sensorType == SensorType.Inside)
            result = new InsideSensor();
        else
            result = new OutsideSensor();
        
        if(_sensors.TryAdd(result.Guid, result))
            return await Task.FromResult(result);

        return await Task.FromResult(default(ISensor));
    }
    
    public async Task<bool> SubscribeToSensor(Guid guid)
    {
        return await Task.FromResult(_sensorsSu.TryAdd(guid, DateTime.UtcNow));
    }

    public async Task<bool> UnsubscribeFromSensor(Guid guid)
    {
        return await Task.FromResult(_sensorsSu.TryRemove(guid, out var sensor));
    }

    private void GetTestData(int count)
    {
        for (int i = 0; i < count; i++)
        {
            //var insideSensor = new InsideSensor();
            var outsideSensor = new OutsideSensor();
            //_sensors.TryAdd(insideSensor.Guid, insideSensor);
            _sensors.TryAdd(outsideSensor.Guid, outsideSensor);
        }
    }
}