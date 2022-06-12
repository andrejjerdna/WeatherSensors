using Ozon256.WeatherSensors.Contracts;
using Ozon256.WeatherSensors.Core.Models;

namespace Ozon256.WeatherSensors.SensorsEmulatorService.Models;

public class SensorFactory
{
    public static Task<ISensor> SensorBuild(SensorType sensorType)
    {
        if (sensorType == SensorType.Inside)
            return Task.FromResult(GetInsideSensor());

        return Task.FromResult(GetOutsideSensor());
    }

    private static ISensor GetInsideSensor() => new InsideSensor();
    
    private static ISensor GetOutsideSensor() => new OutsideSensor();
}