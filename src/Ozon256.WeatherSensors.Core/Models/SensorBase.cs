using Ozon256.WeatherSensors.Contracts;

namespace Ozon256.WeatherSensors.Core.Models;

public abstract class SensorBase : ISensor
{
    private protected readonly Random _random = new Random();
    private protected ISensorData _lastData;
    public Guid Guid  { get; } = Guid.NewGuid();
    public SensorType SensorType { get; init; }
    public virtual ISensorData GetData() => throw new NotImplementedException();
    public ISensorData LastData => _lastData;

    /// <summary>
    /// Generate a new value
    /// </summary>
    /// <param name="currentValue">Current value</param>
    /// <param name="deltaPercent">Delta percent</param>
    /// <param name="maxValue"></param>
    /// <param name="minValue"></param>
    /// <returns></returns>
    private protected int GetNewValue(int currentValue, int deltaPercent, int maxValue, int minValue)
    {
        var delta = (double)_random.Next(-1, 2);
        var newValue = (currentValue + (int)Math.Ceiling((delta * currentValue * deltaPercent / 100)));

        if (newValue < maxValue && newValue > minValue)
            return newValue;

        return currentValue;
    }
}