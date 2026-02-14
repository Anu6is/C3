using C3.Application.Interfaces;
using C3.Domain;
using C3.Domain.Models;
using C3.Infrastructure.TornApi;
using Microsoft.Extensions.Logging;

namespace C3.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ITornApiService _apiService;
    private readonly ILogger<UserRepository> _logger;

    public UserRepository(ITornApiService apiService, ILogger<UserRepository> logger)
    {
        _apiService = apiService;
        _logger = logger;
    }

    public async Task<Result<UserData>> GetCurrentUserAsync()
    {
        var result = await _apiService.GetCurrentUserAsync();

        if (result.IsFailure)
        {
            _logger.LogError("Failed to fetch current user: {Error}", result.Error);
            return Result<UserData>.Failure(result.Error);
        }

        var user = result.Value;
        if (user is null || user.Name is null)
            return Result<UserData>.Failure("Failed to retrieve user data");

        var data = new UserData(
            user.Player_Id,
            user.Name,
            user.Level,
            new UserFactionData(user.Faction.Faction_Id, user.Faction.Faction_Name, user.Faction.Faction_Tag),
            new BattleStatsData(user.Strength, user.Defense, user.Speed, user.Dexterity),
            new UserStateData(user.Status.Description, user.Status.State, user.Status.Until),
            new UserActivityData(user.Last_Action.Status, user.Last_Action.Timestamp));

        return Result<UserData>.Success(data);
    }
}
