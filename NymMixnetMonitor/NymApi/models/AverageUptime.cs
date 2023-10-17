namespace NymMixnetMonitor.NymApi.models
{
    public class NodePerformance
    {
        public string most_recent { get; set; }
        public string last_hour { get; set; }
        public string last_24h { get; set; }
    }

    public class AverageUptime
    {
        public int mix_id { get; set; }
        public int avg_uptime { get; set; }
        public NodePerformance? node_performance { get; set; }
    }
}
