using NymMixnetMonitor.NymApi.models;

namespace NymMixnetMonitor.NymApi
{
    public class NymApiService : INymApiService
    {
        private readonly HttpClient _httpClient;

        public NymApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<Node>> GetAllMixnodes(CancellationToken cancel)
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<Node>>("/api/v1/mixnodes", cancel);
        }

        public async Task<Node> GetMixnode(int mixId, CancellationToken cancel)
        {
            var allNodes = await GetAllMixnodes(cancel);
            return allNodes.FirstOrDefault(_ => _.bond_information.mix_id == mixId);
        }

        public async Task<AverageUptime> GetAverageUptime(int mixnodeId, CancellationToken cancel)
        {
            return await _httpClient.GetFromJsonAsync<AverageUptime>($"/api/v1/status/mixnode/{mixnodeId}/avg_uptime", cancel);
        }

        public async Task<Status> GetStatus(int mixnodeId, CancellationToken cancel)
        {
            return await _httpClient.GetFromJsonAsync<Status>($"/api/v1/status/mixnode/{mixnodeId}/status", cancel);
        }

        public async Task<StakeSaturation> GetStakeSaturation(int mixnodeId, CancellationToken cancel)
        {
            return await _httpClient.GetFromJsonAsync<StakeSaturation>($"/api/v1/status/mixnode/{mixnodeId}/stake-saturation", cancel);
        }

        public async Task<InclusionProbability> GetInclusionProbability(int mixnodeId, CancellationToken cancel)
        {
            return await _httpClient.GetFromJsonAsync<InclusionProbability>($"/api/v1/status/mixnode/{mixnodeId}/inclusion-probability", cancel);
        }
    }
}
