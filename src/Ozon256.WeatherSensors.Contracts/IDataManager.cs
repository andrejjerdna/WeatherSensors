namespace Ozon256.WeatherSensors.Contracts;

public interface IDataManager
{
    Task<ISensorDataSelection> GetSensorDataByGuidForInterval(Guid guid, DateTime startTime, int minutes);
    Task<ISensorDataSelection> GetSensorDataByGuidForInterval(Guid guid, DateTime startTime, DateTime endTime);
}