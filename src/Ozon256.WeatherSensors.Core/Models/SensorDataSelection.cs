using Ozon256.WeatherSensors.Contracts;

namespace Ozon256.WeatherSensors.Core.Models;

public class SensorDataSelection : ISensorDataSelection
{
    public Guid SensorGuid { get; init; }
    public int AverageTemperature { get; init; }
    public int AverageHumidity { get; init; }
    public int AveragePpm { get; init; }
    public DateTime StartTime { get;  init; }
    public DateTime EndTime { get;  init; }
}