using Ozon256.WeatherSensors.Contracts;

namespace Ozon256.WeatherSensors.Core.Models;

public record SensorData : ISensorData
{
    public Guid  SensorGuid { get; init; }
    public int Temperature { get; init; }
    public int Humidity { get; init; }
    public int Ppm { get; init; }
    public DateTime TimeStamp { get; init; }
}