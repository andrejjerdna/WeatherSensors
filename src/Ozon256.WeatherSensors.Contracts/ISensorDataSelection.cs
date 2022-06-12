namespace Ozon256.WeatherSensors.Contracts;

public interface ISensorDataSelection
{
    Guid  SensorGuid { get; }
    int AverageTemperature { get; }
    int AverageHumidity { get; }
    int AveragePpm { get; }
    DateTime StartTime { get; }
    DateTime EndTime { get; }
}