namespace Ozon256.WeatherSensors.Contracts;

public interface ISensor
{
    /// <summary>
    /// Guid
    /// </summary>
    public Guid Guid { get; }
    
    /// <summary>
    /// Sensor type
    /// </summary>
    SensorType SensorType { get; }
    
    /// <summary>
    /// Get sensor data
    /// </summary>
    /// <returns></returns>
    ISensorData GetData();
    
    /// <summary>
    /// Latest sensor data 
    /// </summary>
    ISensorData LastData { get; }
    
    /// <summary>
    /// Add time
    /// </summary>
    DateTime AddTime { get; }
}