using C3.Domain;
using C3.Domain.Models;

namespace C3.Infrastructure.TornApi;

public interface ITornStatsApiService
{
    Task<Result<SpyResults>> GetFactionSpiesAsync(int factionId);
}
