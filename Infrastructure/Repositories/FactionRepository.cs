using C3.Application.Interfaces;
using C3.Domain;
using C3.Domain.Models;
using C3.Infrastructure.TornApi;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace C3.Infrastructure.Repositories;

public class FactionRepository : IFactionRepository
{
    private readonly ITornApiService _apiService;
    private readonly ITornStatsApiService _statsService;
    private readonly ILogger<FactionRepository> _logger;
    private readonly IMemoryCache _cache;

    public FactionRepository(
        ITornApiService apiService,
        ITornStatsApiService statsService,
        ILogger<FactionRepository> logger,
        IMemoryCache cache)
    {
        _apiService = apiService;
        _statsService = statsService;
        _logger = logger;
        _cache = cache;
    }

    public async Task<Result<FactionData>> GetFactionDataAsync(int factionId)
    {
        var cacheKey = $"faction_{factionId}";

        if (_cache.TryGetValue(cacheKey, out FactionData? cached))
        {
            _logger.LogDebug("Returning cached faction data for {FactionId}", factionId);
            return Result<FactionData>.Success(cached!);
        }

        var result = await _apiService.GetFactionAsync(factionId);

        if (result.IsFailure)
        {
            _logger.LogError("Failed to fetch faction {FactionId}: {Error}", factionId, result.Error);
            return Result<FactionData>.Failure(result.Error);
        }

        var faction = result.Value;
        if (faction is null)
            return Result<FactionData>.Failure("Faction not found");

        var data = MapToFactionData(faction);

        // Cache for 5 seconds (faction data changes during war)
        _cache.Set(cacheKey, data, TimeSpan.FromSeconds(5));

        return Result<FactionData>.Success(data);
    }

    public async Task<Result<Dictionary<int, SpyData>>> GetFactionSpiesAsync(int factionId)
    {
        var cacheKey = $"spies_{factionId}";

        if (_cache.TryGetValue(cacheKey, out Dictionary<int, SpyData>? cached))
        {
            return Result<Dictionary<int, SpyData>>.Success(cached!);
        }

        var result = await _statsService.GetFactionSpiesAsync(factionId);

        if (result.IsFailure)
        {
            _logger.LogError("Failed to fetch spies for faction {FactionId}: {Error}", factionId, result.Error);
            return Result<Dictionary<int, SpyData>>.Failure(result.Error);
        }

        var spyResults = result.Value;
        if (spyResults is null || !spyResults.Status)
            return Result<Dictionary<int, SpyData>>.Failure("No spy data available");

        var spies = spyResults.Faction.Members.Values
            .ToDictionary(
                m => m.Id,
                m => new SpyData(
                    m.Id,
                    m.Spy.Strength,
                    m.Spy.Defense,
                    m.Spy.Speed,
                    m.Spy.Dexterity,
                    m.Spy.Total,
                    m.Spy.Timestamp));

        // Cache spy data for 1 minute
        _cache.Set(cacheKey, spies, TimeSpan.FromMinutes(1));

        return Result<Dictionary<int, SpyData>>.Success(spies);
    }

    private static FactionData MapToFactionData(TornFaction faction)
    {
        var members = faction.Members.ToDictionary(
            kvp => kvp.Key,
            kvp => new FactionMemberData(
                kvp.Value.Name,
                kvp.Value.Level,
                kvp.Value.Status.State,
                kvp.Value.Last_Action.Status,
                kvp.Value.Status.Until,
                kvp.Value.Last_Action.Timestamp));

        var wars = faction.Ranked_Wars.Select(kvp =>
        {
            var warScores = kvp.Value.Factions.ToDictionary(
                f => f.Key,
                f => new WarScoreData(f.Value.Name, f.Value.Score, f.Value.Chain));

            return new RankedWarData(
                kvp.Key,
                kvp.Value.War.Start,
                kvp.Value.War.End,
                kvp.Value.War.Target,
                kvp.Value.War.Winner == 0 ? null : kvp.Value.War.Winner,
                warScores);
        }).ToList();

        return new FactionData(
            faction.Id,
            faction.Name,
            faction.Tag,
            faction.Tag_Image,
            members,
            wars);
    }
}
