using C3.Domain;
using C3.Domain.Models;

namespace C3.Infrastructure.TornApi;

public interface ITornApiService
{
    Task<Result<TornUser>> GetCurrentUserAsync();
    Task<Result<TornFaction>> GetFactionAsync(int factionId);
}
