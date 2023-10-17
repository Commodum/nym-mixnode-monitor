namespace NymMixnetMonitor.NymApi.models
{
    public class BondInformation
    {
        public int mix_id { get; set; }
        public string owner { get; set; }
        public OriginalPledge original_pledge { get; set; }
        public int layer { get; set; }
        public MixNode mix_node { get; set; }
        public string proxy { get; set; }
        public int bonding_height { get; set; }
        public bool is_unbonding { get; set; }
    }

    public class CostParams
    {
        public string profit_margin_percent { get; set; }
        public IntervalOperatingCost interval_operating_cost { get; set; }
    }

    public class IntervalOperatingCost
    {
        public string denom { get; set; }
        public string amount { get; set; }
    }

    public class MixNode
    {
        public string host { get; set; }
        public int mix_port { get; set; }
        public int verloc_port { get; set; }
        public int http_api_port { get; set; }
        public string sphinx_key { get; set; }
        public string identity_key { get; set; }
        public string version { get; set; }
    }

    public class OriginalPledge
    {
        public string denom { get; set; }
        public string amount { get; set; }
    }

    public class PendingChanges
    {
        public object pledge_change { get; set; }
    }

    public class RewardingDetails
    {
        public CostParams cost_params { get; set; }
        public string @operator { get; set; }
        public string delegates { get; set; }
        public string total_unit_reward { get; set; }
        public string unit_delegation { get; set; }
        public int last_rewarded_epoch { get; set; }
        public int unique_delegations { get; set; }
    }

    public class Node
    {
        public BondInformation bond_information { get; set; }
        public RewardingDetails rewarding_details { get; set; }
        public PendingChanges pending_changes { get; set; }
    }


}
