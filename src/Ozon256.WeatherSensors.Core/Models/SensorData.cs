using System.Text;
using Ozon256.WeatherSensors.Contracts;

namespace Ozon256.WeatherSensors.Core.Models;

public record SensorData : ISensorData
{
    public Guid  SensorGuid { get; init; }
    public int Temperature { get; init; }
    public int Humidity { get; init; }
    public int Ppm { get; init; }
    public DateTime UpdateTime { get; init; }

    public override string ToString()
    {
        return new StringBuilder()
            .Append($"Sensor GUID: {SensorGuid}")
            .Append(Environment.NewLine)
            .Append($"Temperature: {Temperature}")
            .Append(Environment.NewLine)
            .Append($"Humidity: {Humidity}")
            .Append(Environment.NewLine)
            .Append($"Ppm: {Ppm}")
            .ToString();
    }
}