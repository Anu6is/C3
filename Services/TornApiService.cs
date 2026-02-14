using C3.Models;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;
using System.Text.Json;

namespace C3.Services;

public sealed class TornApiService(HttpClient httpClient, ProtectedTokenStore TokenStore, ILogger<TornApiService> logger) : IDisposable
{
    private static readonly JsonSerializerOptions _jsonOptions = new() { PropertyNameCaseInsensitive = true };

    public async Task<Result<TornUser>> GetCurrentUserAsync()
    {
        var key = await TokenStore.GetTokenAsync();
        if (key is null) return Result<TornUser>.Failure("No API key available");

        return await ExecuteRequestAsync<TornUser>(Endpoints.User().Profile().BattleStats().WithAuthorization(key));
    }

    public async Task<Result<TornFaction>> GetFactionAsync(int factionId)
    {
        var key = await TokenStore.GetTokenAsync();
        if (key is null) return Result<TornFaction>.Failure("No API key available");

        return await ExecuteRequestAsync<TornFaction>(Endpoints.Faction(factionId).WithAuthorization(key));
    }

    private async Task<Result<T>> ExecuteRequestAsync<T>(string url)
    {
        try
        {
            var response = await httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode)
            {
                return Result<T>.Failure($"HTTP error: {response.StatusCode}");
            }

            var content = await response.Content.ReadAsStringAsync();

            using var doc = JsonDocument.Parse(content);
            if (doc.RootElement.TryGetProperty("error", out var errorElement))
            {
                var error = JsonSerializer.Deserialize<TornError>(errorElement.GetRawText(), _jsonOptions);
                return Result<T>.Failure(error?.Error ?? "Unknown API error");
            }

            var result = JsonSerializer.Deserialize<T>(content, _jsonOptions);
            return result != null
                ? Result<T>.Success(result)
                : Result<T>.Failure("Failed to retrieve data from API");
        }
        catch (HttpRequestException ex)
        {
            return Result<T>.Failure($"Network error: {ex.Message}");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error during Torn API request to {Url}", url);
            return Result<T>.Failure($"Unexpected error: {ex.Message}");
        }
    }

    public void Dispose() => httpClient.Dispose();
}
