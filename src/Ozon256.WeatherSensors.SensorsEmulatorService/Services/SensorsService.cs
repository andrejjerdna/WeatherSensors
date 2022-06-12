using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.Extensions.Options;
using Ozon256.WeatherSensors.Contracts;
using Ozon256.WeatherSensors.DataProcessorClient;
using Ozon256.WeatherSensors.SensorsEmulatorService;
using Ozon256.WeatherSensors.SensorsEmulatorService.Extensions;
using Ozon256.WeatherSensors.SensorsEmulatorService.Models;
using Ozon256.WeatherSensors.SensorsEmulatorService.Options;
using SensorType = Ozon256.WeatherSensors.DataProcessorClient.SensorTypeRequest;

namespace Ozon256.WeatherSensors.SensorsEmulatorService.Services;

public class SensorsService : Sensors.SensorsBase
{
    private readonly ISensorsPool _sensorsPool;
    private readonly IOptions<SensorsPoolConfig> _sensorsPoolConfig;
    private readonly IServiceProvider _provider;
    private readonly ILogger<SensorsService> _logger;
    
    /// <summary>
    /// .ctor
    /// </summary>
    /// <param name="sensorsPool"></param>
    /// <param name="sensorsPoolConfig"></param>
    /// <param name="provider"></param>
    /// <param name="logger"></param>
    public SensorsService(ISensorsPool sensorsPool, IOptions<SensorsPoolConfig> sensorsPoolConfig, IServiceProvider provider, ILogger<SensorsService> logger)
    {
        _sensorsPool = sensorsPool;
        _sensorsPoolConfig = sensorsPoolConfig;
        _provider = provider;
        _logger = logger;
    }
    
    public override async Task GetSensorsData(IAsyncStreamReader<ActionMessage> requestStream, 
        IServerStreamWriter<SensorsDataCollectionResponse> responseStream, 
        ServerCallContext context)
    {
        while (!context.CancellationToken.IsCancellationRequested)
        {
            try
            {
                var dataProcessorToServer = DataProcessorToServer(requestStream, context);
                var serverToDataProcessor = ServerToDataProcessor(responseStream, context);

                await Task.WhenAll(dataProcessorToServer, serverToDataProcessor);
            }
            catch (Exception e)
            {
                _logger.LogWarning(e.Message);
            }
        }
    }

    public override async Task<SensorsDataCollectionResponse> GetAllSensors(Empty request, ServerCallContext context)
    {
        var sensorsDataCollectionResponse = new SensorsDataCollectionResponse();
        
        var sensorsData = await _sensorsPool.GetSensorsData();

        foreach (var sensorData in sensorsData)
        {
            sensorsDataCollectionResponse.SensorDataResponse.Add(sensorData.GetSensorDataResponse());
        }
        
        return await Task.FromResult(sensorsDataCollectionResponse);
    }
    
    public override async Task<Empty> RemoveSensor(RemoveSensorRequest request, ServerCallContext context)
    {
        await _sensorsPool.DeleteSensorsByGuid(new Guid(request.Guid));
        return new Empty();
    }
    
    public override async Task<SensorDataResponse> AddSensor(AddSensorRequest request, ServerCallContext context)
    {
        ISensor result;
        if (request.SensorType == SensorTypeRequest.Outside)
            result = await _sensorsPool.AddNewSensor(Contracts.SensorType.Outside);
        else
            result = await _sensorsPool.AddNewSensor(Contracts.SensorType.Inside);

        return result.GetSensorDataResponse();
    }
    
    public override async Task<SensorDataResponse> GetSensorByGuid(GetSensorByGuidRequest request, ServerCallContext context)
    {
        var sensorsData = await _sensorsPool.GetSensorsData();

        var sensorData = sensorsData.FirstOrDefault(s => s.SensorGuid.ToString() == request.Guid);
        
        if(sensorData == null)
            return await Task.FromResult(default(SensorDataResponse));
        
        return await Task.FromResult(sensorData.GetSensorDataResponse());
    }
    
    private async Task ServerToDataProcessor(IServerStreamWriter<SensorsDataCollectionResponse> responseStream, ServerCallContext context)
    {
        while (!context.CancellationToken.IsCancellationRequested)
        {
            var sensorsData = await _sensorsPool.GetSensorsData();

            var sensorsDataCollection = new SensorsDataCollectionResponse();

            foreach (var data in sensorsData)
                sensorsDataCollection.SensorDataResponse.Add(data.GetSensorDataResponse());

            await responseStream.WriteAsync(sensorsDataCollection, context.CancellationToken);
            await Task.Delay(_sensorsPoolConfig.Value.UpdateTime);
        }
    }

    private async Task DataProcessorToServer(IAsyncStreamReader<ActionMessage> requestStream, ServerCallContext context)
    {
        while (await requestStream.MoveNext() && !context.CancellationToken.IsCancellationRequested)
        {
            var actionMessage = requestStream.Current;
                
            var add = actionMessage.Add.ToArray();

            foreach (var a in add)
                await _sensorsPool.SubscribeToSensor(new Guid(a));

            var remove = actionMessage.Remove.ToArray();
            
            foreach (var r in remove)
                await _sensorsPool.UnsubscribeFromSensor(new Guid(r));
        }
    }
}