using NymMixnetMonitor.MixnodeFacade.models;

namespace NymMixnetMonitor.MixnodeFacade
{
    public class MixnodeApiService : IMixnodeApiService
    {
        private readonly HttpClient _httpClient;

        public MixnodeApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<Stats> GetStats(CancellationToken cancel)
        {
            return await _httpClient.GetFromJsonAsync<Stats>("/stats", cancel);
        }
    }
}
