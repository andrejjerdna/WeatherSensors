using Ozon256.WeatherSensors.Contracts;
using Ozon256.WeatherSensors.Core.Models;

namespace Ozon256.WeatherSensors.Core.Models;

public class InsideSensor : SensorBase
{
    private int _temperature;
    private int _humidity;
    private int _ppm;

    public InsideSensor()
    { 
        _temperature = _random.Next(15, 40);
        _humidity = _random.Next(30, 80);
        _ppm = _random.Next(400, 900);

        SensorType = SensorType.Inside;
    }

    public override ISensorData GetData()
    {
        _temperature = GetNewValue(_temperature, 10, 30, 10);
        _humidity = GetNewValue(_humidity, 15, 80, 30);
        _ppm = GetNewValue(_ppm, 40, 1000, 400);
        
        _lastData = new SensorData
        {
            SensorGuid = Guid,
            TimeStamp = DateTime.Now.ToUniversalTime(),
            Temperature = _temperature,
            Humidity = _humidity,
            Ppm = _ppm,
        };

        return _lastData;
    }
}