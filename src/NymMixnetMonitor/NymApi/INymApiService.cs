using NymMixnetMonitor.NymApi.models;

namespace NymMixnetMonitor.NymApi
{
    public interface INymApiService
    {
        Task<IEnumerable<Node>> GetAllMixnodes(CancellationToken cancel);
        Task<Node> GetMixnode(int mixId, CancellationToken cancel);
        Task<AverageUptime> GetAverageUptime(int mixnodeId, CancellationToken cancel);
        Task<Status> GetStatus(int mixnodeId, CancellationToken cancel);
        Task<StakeSaturation> GetStakeSaturation(int mixnodeId, CancellationToken cancel);
        Task<InclusionProbability> GetInclusionProbability(int mixnodeId, CancellationToken cancel);
    }
}
