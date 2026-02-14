using C3.Domain;
using C3.Domain.Models;

namespace C3.Application.Interfaces;

public interface IFactionRepository
{
    Task<Result<FactionData>> GetFactionDataAsync(int factionId);
    Task<Result<Dictionary<int, SpyData>>> GetFactionSpiesAsync(int factionId);
}
