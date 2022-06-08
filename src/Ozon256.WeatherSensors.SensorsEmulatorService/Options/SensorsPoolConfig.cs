using Microsoft.Extensions.Options;

namespace Ozon256.WeatherSensors.SensorsEmulatorService.Options;

public class SensorsPoolConfig
{
    public const string SensorsPool = "SensorsPoolConfig";
    public int UpdateTime { get; set; }

}