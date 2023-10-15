using Prometheus;
using System.Diagnostics;
using System.Net.Http;

namespace NymMixnetMonitor
{
    /// <summary>
    /// https://github.com/prometheus-net/prometheus-net/blob/master/Sample.Web/SampleService.cs
    /// </summary>
    public sealed class WorkerService : BackgroundService
    {
        private const int generalDelayInSeconds = 10;
        private static readonly Counter IterationCount = Metrics.CreateCounter("nymMixnet_iterations_total", "Number of iterations that the sample service has ever executed.");

        protected override async Task ExecuteAsync(CancellationToken cancel)
        {
            try
            {
                while (!cancel.IsCancellationRequested)
                {
                    try
                    {
                        await ReadySetGoAsync(cancel);
                    }
                    catch
                    {
                        // Something failed? OK, whatever. We will just try again.
                    }

                    await Task.Delay(TimeSpan.FromSeconds(generalDelayInSeconds), cancel);
                }
            }
            catch (OperationCanceledException) when (cancel.IsCancellationRequested)
            {
            }
        }

        private async Task ReadySetGoAsync(CancellationToken cancel)
        {
            IterationCount.Inc();
        }
    }
}
