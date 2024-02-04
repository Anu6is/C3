using C3.Models;
using System.Net.Http.Json;

namespace C3.Services;

public sealed class TornApiService(HttpClient httpClient, ProtectedTokenStore TokenStore) : IDisposable
{
    public async Task<TornUser?> GetCurrentUserAsync()
    {
        var key = await TokenStore.GetTokenAsync();

        return await httpClient.GetFromJsonAsync<TornUser>($"user/?selections=profile&key={key}");
    }

    public void Dispose() => httpClient.Dispose();
}