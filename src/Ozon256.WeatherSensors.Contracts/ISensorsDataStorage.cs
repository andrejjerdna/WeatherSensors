namespace Ozon256.WeatherSensors.Contracts;

public interface ISensorsDataStorage
{
    void Add(ISensorData sensorData);
    void RemoveSensor(Guid guid);
    void AddSensor(Guid guid);
    
    
}