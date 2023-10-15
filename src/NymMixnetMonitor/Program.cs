using NymMixnetMonitor;
using Prometheus;

var builder = WebApplication.CreateBuilder(args);

// configure woker service
builder.Services.AddHostedService<WorkerService>();

var app = builder.Build();

app.UseRouting();
app.UseEndpoints(endpoints =>
    {
        endpoints.MapMetrics();
    });

Metrics.SuppressDefaultMetrics();

app.Run();
