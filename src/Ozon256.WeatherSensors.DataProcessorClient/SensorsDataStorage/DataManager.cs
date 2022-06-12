using Ozon256.WeatherSensors.Contracts;
using Ozon256.WeatherSensors.Core.Models;

namespace Ozon256.WeatherSensors.DataProcessorClient.SensorsDataStorage;

public class DataManager : IDataManager
{
    private readonly ISensorsDataStorage _sensorsDataStorage;

    public DataManager(ISensorsDataStorage sensorsDataStorage)
    {
        _sensorsDataStorage = sensorsDataStorage;
    }
    
    public async Task<ISensorDataSelection> GetSensorDataByGuidForInterval(Guid guid, DateTime startTime, DateTime endTime)
    {
        var sensorsData = await _sensorsDataStorage.GetSensorDataByGuid(guid);

        if (sensorsData is null)
            return null;

        var sensorDatas = sensorsData.ToList();
        var dataCount = sensorDatas.Count();

        var averageTemperature = sensorDatas.Select(s => s.Temperature).Sum() / dataCount;
        var averageHumidity = sensorDatas.Select(s => s.Humidity).Sum() / dataCount;
        var averagePpm = sensorDatas.Select(s => s.Ppm).Sum() / dataCount;
        
        return new SensorDataSelection
        {
            SensorGuid = guid,
            AverageTemperature = averageTemperature,
            AverageHumidity = averageHumidity,
            AveragePpm = averagePpm,
            StartTime = startTime,
            EndTime = endTime
        };
    }
    
    public async Task<ISensorDataSelection> GetSensorDataByGuidForInterval(Guid guid, DateTime startTime, int minutes)
    {
        var endTime = startTime + TimeSpan.FromMinutes(minutes);

        return await GetSensorDataByGuidForInterval(guid, startTime, endTime);
    }
}