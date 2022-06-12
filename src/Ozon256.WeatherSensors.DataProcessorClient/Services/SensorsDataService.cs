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
    private readonly ILogger<SensorsService> _logger;
    private readonly Sensors.SensorsClient _client;
    private readonly Guid _clientGuid = Guid.NewGuid();

    public SensorsService(ISensorsDataStorage sensorsDataStorage, IServiceProvider provider, ILogger<SensorsService> logger, Sensors.SensorsClient client)
    {
        _sensorsDataStorage = sensorsDataStorage;
        _provider = provider;
        _logger = logger;
        _client = client;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await Task.Run(async () =>
        {
            AsyncDuplexStreamingCall<ActionMessage, SensorsDataCollectionResponse> stream = null;

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    stream = _client.GetSensorsData(cancellationToken: stoppingToken);

                    var write = WriteToStream(stream.RequestStream, stoppingToken);
                    var response = ReadFromStream(stream.ResponseStream, stoppingToken);

                    Task.WaitAll(write, response);
                }
                catch (Exception e)
                {
                    stream?.Dispose();
                    _logger.LogWarning(e.Message);
                    await Task.Delay(5000);
                }
            }

        }, stoppingToken);
    }

    private async Task ReadFromStream(IAsyncStreamReader<SensorsDataCollectionResponse> responseStream, CancellationToken context)
    {
        try
        {
            await foreach (var response in responseStream.ReadAllAsync(cancellationToken: context))
            {
                if(response is null)
                    continue;

                foreach (var e in response.SensorDataResponse)
                {
                    if(e is null)
                        continue;
                    
                    var data = new SensorData()
                    {
                        SensorGuid = new Guid(e.Guid),
                        Temperature = e.Temperature,
                        Humidity = e.Humidity,
                        Ppm = e.Ppm,
                        UpdateTime = e.Updatetime.ToDateTime()
                    };
                    
                    _sensorsDataStorage.Add(data);
                    
                    _logger.LogInformation(data.ToString());
                }
            }
        }
        catch (Exception e)
        {
            _logger.LogWarning(e.Message);
        }
    }

    private async Task WriteToStream(IClientStreamWriter<ActionMessage> requestStream, CancellationToken context)
    {
        var actionMessage = new ActionMessage
        {
            Clientguid = _clientGuid.ToString(),
        };
        
        while(_sensorsDataStorage.SubscriptionSensors.TryDequeue(out var guid))
            actionMessage.Add.Add(guid.ToString());
        
        while(_sensorsDataStorage.UnSubscriptionSensors.TryDequeue(out var guid))
            actionMessage.Add.Add(guid.ToString());
        
        await requestStream.WriteAsync(actionMessage);
        await requestStream.CompleteAsync();
    }
}