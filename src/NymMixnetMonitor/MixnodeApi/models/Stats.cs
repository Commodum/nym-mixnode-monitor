namespace NymMixnetMonitor.MixnodeFacade.models
{
    public class Stats
    {
        public string update_time { get; set; }
        public string previous_update_time { get; set; }
        public int packets_received_since_startup { get; set; }
        public int packets_sent_since_startup { get; set; }
        public int packets_explicitly_dropped_since_startup { get; set; }
        public int packets_received_since_last_update { get; set; }
        public int packets_sent_since_last_update { get; set; }
        public int packets_explicitly_dropped_since_last_update { get; set; }
    }
}
