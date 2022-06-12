using Ozon256.WeatherSensors.Contracts;
using Ozon256.WeatherSensors.Core.Models;

namespace Ozon256.WeatherSensors.Core.Models;

public sealed class OutsideSensor : SensorBase
{
    private int _temperature;
    private int _humidity;
    private int _ppm;

    public OutsideSensor()
    { 
        _temperature = _random.Next(-20, 50);
        _humidity = _random.Next(10, 90);
        _ppm = _random.Next(300, 400);

        AddTime = DateTime.Now.ToUniversalTime();
        SensorType = SensorType.Outside;
    }

    public override ISensorData GetData()
    {
        _temperature = GetNewValue(_temperature, 10, 50, -20);
        _humidity = GetNewValue(_humidity, 20, 100, 10);
        _ppm = GetNewValue(_ppm, 5, 500, 300);
        
        _lastData = new SensorData
        {
            SensorGuid = Guid,
            UpdateTime = DateTime.Now.ToUniversalTime(),
            Temperature = _temperature,
            Humidity = _humidity,
            Ppm = _ppm,
        };
        
        return _lastData;
    }
}