namespace Ozon256.WeatherSensors.Contracts;

public interface ISensorsDataStorage
{
    Queue<Guid> SubscriptionSensors { get; }
    Queue<Guid> UnSubscriptionSensors { get; }
    Task<bool> AddSubscriptionSensor(Guid guid);
    Task<bool>  AddUnsubscriptionSensor(Guid guid);
    void Add(ISensorData sensorData);
    Task<IEnumerable<ISensorData>?> GetSensorDataByGuid(Guid guid);
}