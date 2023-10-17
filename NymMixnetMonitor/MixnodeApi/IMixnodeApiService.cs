using NymMixnetMonitor.MixnodeFacade.models;

namespace NymMixnetMonitor.MixnodeFacade
{
    public interface IMixnodeApiService
    {
        Task<Stats> GetStats(CancellationToken cancel);
    }
}
