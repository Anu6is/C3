using C3.Models;
using System.Net.Http.Json;

namespace C3.Services;

public sealed class TornApiService(HttpClient httpClient, ProtectedTokenStore TokenStore) : IDisposable
{
    public async Task<TornUser?> GetCurrentUserAsync()
    {
        var key = await TokenStore.GetTokenAsync();
        
        if (key is null) return null;

        return await httpClient.GetFromJsonAsync<TornUser>(Endpoints.User().Profile().WithAuthorization(key));
    }

    public async Task<TornFaction?> GetFactionAsync(int factionId)
    {
        var key = await TokenStore.GetTokenAsync();

        if (key is null) return null;

        return await httpClient.GetFromJsonAsync<TornFaction>(Endpoints.Faction(factionId).WithAuthorization(key));
    }

    public void Dispose() => httpClient.Dispose();
}