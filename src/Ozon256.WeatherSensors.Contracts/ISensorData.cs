namespace Ozon256.WeatherSensors.Contracts;

public interface ISensorData
{
    Guid  SensorGuid { get; }
    int Temperature { get; }
    int Humidity { get; }
    int Ppm { get; }
    DateTime TimeStamp { get; }
}