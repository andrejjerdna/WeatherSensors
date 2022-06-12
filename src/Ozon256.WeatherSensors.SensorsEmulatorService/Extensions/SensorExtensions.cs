using Google.Protobuf.WellKnownTypes;
using Ozon256.WeatherSensors.Contracts;
using Ozon256.WeatherSensors.DataProcessorClient;

namespace Ozon256.WeatherSensors.SensorsEmulatorService.Extensions;

public static class SensorExtensions
{
    public static SensorDataResponse GetSensorDataResponse(this ISensor sensor)
    {
        return sensor.GetData().GetSensorDataResponse();
    }
    
    public static SensorDataResponse GetSensorDataResponse(this ISensorData sensorData)
    {
        return new SensorDataResponse
        {
            Guid = sensorData.SensorGuid.ToString(),
            Humidity = sensorData.Humidity,
            Temperature = sensorData.Temperature,
            Ppm = sensorData.Ppm,
            Updatetime = sensorData.UpdateTime.ToTimestamp()
        };
    }
}