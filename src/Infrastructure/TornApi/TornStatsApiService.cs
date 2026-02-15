using C3.Domain;
using C3.Domain.Models;
using C3.Infrastructure.Storage;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;

namespace C3.Infrastructure.TornApi;

public class TornStatsApiService(HttpClient httpClient, ProtectedTokenStore TokenStore, ILogger<TornStatsApiService> logger) : ITornStatsApiService, IDisposable
{
    public async Task<Result<SpyResults>> GetFactionSpiesAsync(int factionId)
    {
        try
        {
            var key = await TokenStore.GetTokenAsync();
            if (key is null) return Result<SpyResults>.Failure("No API key available");

            var result = await httpClient.GetFromJsonAsync<SpyResults>(Endpoints.FactionSpies(factionId).WithAuthorization(key));

            if (result is null)
                return Result<SpyResults>.Failure("Failed to retrieve spy data");

            if (!result.Status)
                return Result<SpyResults>.Failure("TornStats API returned failure status");

            return Result<SpyResults>.Success(result);
        }
        catch (HttpRequestException ex)
        {
            return Result<SpyResults>.Failure($"Network error: {ex.Message}");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error during TornStats API request");
            return Result<SpyResults>.Failure($"Unexpected error: {ex.Message}");
        }
    }

    public void Dispose() => httpClient.Dispose();
}
