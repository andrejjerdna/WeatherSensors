using Google.Protobuf.Collections;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Ozon256.WeatherSensors.Contracts;
using Ozon256.WeatherSensors.Core.Models;
using Ozon256.WeatherSensors.SensorsEmulatorService;

namespace Ozon256.WeatherSensors.DataProcessorClient.Services;

public class SensorsService : BackgroundService
{
    private readonly ISensorsDataStorage _sensorsDataStorage;
    private readonly IServiceProvider _provider;

    public SensorsService(ISensorsDataStorage sensorsDataStorage, IServiceProvider provider)
    {
        _sensorsDataStorage = sensorsDataStorage;
        _provider = provider;
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await using var scope = _provider.CreateAsyncScope();
        var client = scope.ServiceProvider.GetRequiredService<Sensors.SensorsClient>();
           
        while (!stoppingToken.IsCancellationRequested)
        {
            using var eventResponseStream = client.GetSensorsData();

            while (eventResponseStream != null && eventResponseStream.ResponseStream.MoveNext() != null)
            {
                var e = eventResponseStream.ResponseStream.Current;

                var data = new SensorData()
                {
                    SensorGuid = new Guid(e.Guid),
                    Temperature = e.Temperature,
                    Humidity = e.Humidity,
                    Ppm = e.Ppm,
                    TimeStamp = e.Timestamp.ToDateTime()
                };

                var re = new SensorDataRequest();
                re.Addinsidesensorscount = 1;
                await eventResponseStream.RequestStream.WriteAsync(re, stoppingToken);

                _sensorsDataStorage.Add(data);
            }
        }
    }
}