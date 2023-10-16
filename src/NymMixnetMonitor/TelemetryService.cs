using NymMixnetMonitor.MixnodeFacade;
using NymMixnetMonitor.NymApi;
using NymMixnetMonitor.NymApi.models;
using Prometheus;
using System;
using System.Diagnostics;
using System.Net.Http;

namespace NymMixnetMonitor
{
    /// <summary>
    /// https://github.com/prometheus-net/prometheus-net/blob/master/Sample.Web/SampleService.cs
    /// </summary>
    public sealed class TelemetryService : BackgroundService
    {
        private readonly INymApiService _nymApiService;
        private readonly IMixnodeApiService _mixnodeService;
        private readonly int _mixnodeId;
        private readonly Node _mixnode;
        private const int generalDelayInSeconds = 10;
        private static readonly string _telemetryPrefix = "nymMixnode_";

        private static readonly Counter UpdateTime = Metrics.CreateCounter($"{_telemetryPrefix}update_time", "Time of the last update");
        private static readonly Counter IterationCount = Metrics.CreateCounter($"{_telemetryPrefix}iterations_total", "Number of iterations that the sample service has ever executed.");
        private static readonly Counter PacketsReceivedSinceStartup = Metrics.CreateCounter($"{_telemetryPrefix}packets_received_since_startup", "Number of packets received since the node start up.");
        private static readonly Counter PacketsSentSinceStartup = Metrics.CreateCounter($"{_telemetryPrefix}packets_sent_since_startup", "Number of packets sent since the node start up.");
        private static readonly Counter PacketsExplicitlyDroppedSinceStartup = Metrics.CreateCounter($"{_telemetryPrefix}packets_explicitly_dropped_since_startup", "Number of packets explicitly dropped since the node start up.");
        private static readonly Gauge PacketsReceivedSinceLastUpdate = Metrics.CreateGauge($"{_telemetryPrefix}packets_received_since_last_update", "Number of packets received since the last update.");
        private static readonly Gauge PacketsSentSinceLastUpdate = Metrics.CreateGauge($"{_telemetryPrefix}packets_sent_since_last_update", "Number of packets sent since the last update.");
        private static readonly Gauge PacketsExplicitlyDroppedSinceLastUpdate = Metrics.CreateGauge($"{_telemetryPrefix}packets_explicitly_dropped_since_last_update", "Number of packets explicitly dropped since the last update.");
        private static readonly Gauge AvgUptime = Metrics.CreateGauge($"{_telemetryPrefix}average_uptime", "Uptime averaged over the last 24 hours");
        private static readonly Gauge PerformanceMostRecent = Metrics.CreateGauge($"{_telemetryPrefix}performance_most_recent", "Most recent performance");
        private static readonly Gauge PerformanceLastHour = Metrics.CreateGauge($"{_telemetryPrefix}performance_last_hour", "Performance for the last hour");
        private static readonly Gauge PerformanceLast24Hours = Metrics.CreateGauge($"{_telemetryPrefix}performance_last_24_hours", "Performance over the last 24 hours");
        private static readonly Gauge Status = Metrics.CreateGauge($"{_telemetryPrefix}status", "Current status, 1 = active, 0 = inactive.");
        private static readonly Gauge StakeSaturation = Metrics.CreateGauge($"{_telemetryPrefix}stake_saturation", "Current stake saturation");
        private static readonly Gauge StakeSaturationUncapped = Metrics.CreateGauge($"{_telemetryPrefix}stake_saturation_uncapped", "Current uncapped stake saturation");

        public TelemetryService(INymApiService nymApiService, IMixnodeApiService mixnodeService, int mixNodeId)
        {
            _nymApiService = nymApiService;
            _mixnodeService = mixnodeService;
            _mixnodeId = mixNodeId;
            Console.WriteLine($"Obtaining meta data for mixnode: {_mixnodeId}");
            _mixnode = _nymApiService.GetMixnode(_mixnodeId, new CancellationToken()).Result;
            Console.WriteLine($"Mixnode IP: {_mixnode.bond_information.mix_node.host}");
        }

        protected override async Task ExecuteAsync(CancellationToken cancel)
        {
            try
            {
                while (!cancel.IsCancellationRequested)
                {
                    try
                    {
                        await UpdateStatsTelemetry(cancel);
                        await UpdateAvgUptimeTelemetry(cancel);
                        await UpdateStatusTelemetry(cancel);
                        await UpdateStakeSaturationTelemetry(cancel);
                        IterationCount.Inc(1);
                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine(ex);
                    }

                    await Task.Delay(TimeSpan.FromSeconds(generalDelayInSeconds), cancel);
                }
            }
            catch (OperationCanceledException) when (cancel.IsCancellationRequested)
            {
            }
        }

        private async Task UpdateStatsTelemetry(CancellationToken cancel)
        {
            var stats = await _mixnodeService.GetStats(cancel);
            long unixTime = ((DateTimeOffset)DateTime.Parse(stats.update_time)).ToUnixTimeSeconds();
            UpdateTime.IncTo(double.Parse(unixTime.ToString()));
            PacketsReceivedSinceStartup.IncTo(double.Parse(stats.packets_sent_since_startup.ToString()));
            PacketsSentSinceStartup.IncTo(double.Parse(stats.packets_sent_since_startup.ToString()));
            PacketsExplicitlyDroppedSinceStartup.IncTo(double.Parse(stats.packets_explicitly_dropped_since_startup.ToString()));
            PacketsReceivedSinceLastUpdate.Set(double.Parse(stats.packets_sent_since_last_update.ToString()));
            PacketsSentSinceLastUpdate.Set(double.Parse(stats.packets_sent_since_last_update.ToString()));
            PacketsExplicitlyDroppedSinceLastUpdate.Set(double.Parse(stats.packets_explicitly_dropped_since_last_update.ToString()));
            Console.WriteLine($"{stats.update_time}, {stats.packets_received_since_startup}");
        }

        private async Task UpdateAvgUptimeTelemetry(CancellationToken cancel)
        {
            var avgUptime = await _nymApiService.GetAverageUptime(_mixnodeId, cancel);
            AvgUptime.Set(double.Parse(avgUptime.avg_uptime.ToString()));
            PerformanceMostRecent.Set(double.Parse(avgUptime.node_performance.most_recent.ToString()));
            PerformanceLastHour.Set(double.Parse(avgUptime.node_performance.last_hour.ToString()));
            PerformanceLast24Hours.Set(double.Parse(avgUptime.node_performance.last_24h.ToString()));
        }

        private async Task UpdateStatusTelemetry(CancellationToken cancel)
        {
            var status = await _nymApiService.GetStatus(_mixnodeId, cancel);
            var statusAsInt = 0;
            if (status.status == "active") { statusAsInt = 1; }
            Status.Set(statusAsInt);
        }

        private async Task UpdateStakeSaturationTelemetry(CancellationToken cancel)
        {
            var stakeSaturation = await _nymApiService.GetStakeSaturation(_mixnodeId, cancel);
            StakeSaturation.Set(double.Parse(stakeSaturation.saturation.ToString()));
            StakeSaturationUncapped.Set(double.Parse(stakeSaturation.uncapped_saturation.ToString()));
        }
    }
}
