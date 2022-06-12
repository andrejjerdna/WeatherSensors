using Grpc.Core;
using Grpc.Net.Client.Configuration;
using Ozon256.WeatherSensors.Contracts;
using Ozon256.WeatherSensors.DataProcessorClient.Configs;
using Ozon256.WeatherSensors.DataProcessorClient.SensorsDataStorage;
using Ozon256.WeatherSensors.DataProcessorClient.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddGrpc();
builder.Services.AddMvcCore();
builder.Services.AddGrpcClient<Ozon256.WeatherSensors.DataProcessorClient.Sensors.SensorsClient>(
        options =>
        {
            options.Address = new Uri("https://localhost:7134/");
        })
    .ConfigureChannel(o =>
    {
        o.ServiceConfig = ClientConfig.GetServiceConfig();
    });

builder.Services.AddSingleton<ISensorsDataStorage, SensorsDataStorage>();
builder.Services.AddSingleton<IDataManager, DataManager>();
builder.Services.AddHostedService<SensorsService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseRouting();
app.UseEndpoints(
    b =>
    {
        b.MapControllers();
        app.MapGrpcService<SensorsService>();
    });

app.Run();