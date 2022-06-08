namespace Ozon256.WeatherSensors.Contracts;

public interface ISensorsPool
{
    Task<IEnumerable<Guid>> GetAllSensorsGuids();
    Task<IEnumerable<ISensorData>> GetSensorsData();
    Task<ISensor?> GetSensorsByGuid(Guid guid);
    Task<bool> DeleteSensorsByGuid(Guid guid);
    Task<bool> AddSensors(ISensor sensor);
}