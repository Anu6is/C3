using C3.Models;
using System.Net.Http.Json;

namespace C3.Services;

public class TornStatsApiService(HttpClient httpClient, ProtectedTokenStore TokenStore) : IDisposable
{
    public async Task<SpyResults?> GetFactionSpiesAsync(int factionId)
    {
        var key = await TokenStore.GetTokenAsync();

        if (key is null) return null;

        return await httpClient.GetFromJsonAsync<SpyResults>(Endpoints.FactionSpies(factionId).WithAuthorization(key));
    }

    public void Dispose() => httpClient.Dispose();
}
