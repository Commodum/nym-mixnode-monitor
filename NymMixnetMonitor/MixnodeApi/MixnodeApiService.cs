using NymMixnetMonitor.MixnodeFacade.models;

namespace NymMixnetMonitor.MixnodeFacade
{
    public class MixnodeApiService : IMixnodeApiService
    {
        private readonly ILogger<MixnodeApiService> _logger;
        private readonly HttpClient _httpClient;

        public MixnodeApiService(ILogger<MixnodeApiService> logger, HttpClient httpClient)
        {
            _logger = logger;
            _httpClient = httpClient;
            _logger.LogInformation($"Using {_httpClient.BaseAddress.AbsoluteUri} as the base Uri for the Minxnode API.");
        }

        public async Task<Stats> GetStats(CancellationToken cancel)
        {
            return await _httpClient.GetFromJsonAsync<Stats>("/stats", cancel);
        }
    }
}
