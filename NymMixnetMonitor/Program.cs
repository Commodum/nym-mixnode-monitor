using Microsoft.Extensions.DependencyInjection;
using NymMixnetMonitor;
using NymMixnetMonitor.NymApi;
using NymMixnetMonitor.MixnodeFacade;
using Prometheus;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient<INymApiService, NymApiService>(client =>
    {
        client.BaseAddress = new Uri(builder.Configuration["NymApiBaseUrl"]);
    }).SetHandlerLifetime(TimeSpan.FromMinutes(5));

builder.Services.AddHttpClient<IMixnodeApiService, MixnodeApiService>(client =>
    {
        client.BaseAddress = new Uri($"{builder.Configuration["mixnodeScheme"]}://{builder.Configuration["MixnodeIp"]}:{builder.Configuration["MixnodeApiPort"]}/");
    }).SetHandlerLifetime(TimeSpan.FromMinutes(5)); ;

// configure woker service
builder.Services.AddHostedService<TelemetryService>(serviceProvider => new TelemetryService(
    logger: serviceProvider.GetRequiredService<ILogger<TelemetryService>>(),
    nymApiService: serviceProvider.GetRequiredService<INymApiService>(),
    mixnodeService: serviceProvider.GetRequiredService<IMixnodeApiService>(),
    mixNodeId: int.Parse(builder.Configuration["MixnodeId"]))
);

var app = builder.Build();

app.UseRouting();
app.UseEndpoints(endpoints =>
    {
        endpoints.MapMetrics();
    });

Metrics.SuppressDefaultMetrics();

app.Run();
