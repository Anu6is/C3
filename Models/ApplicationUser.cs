namespace C3.Models;

public sealed class ApplicationUser(int userId, string userName, TornUserFaction userFaction, BattleStats stats, string token)
{
    public int UserId { get; } = userId;
    public string Username { get; } = userName;
    public TornUserFaction Faction { get; } = userFaction;
    public BattleStats Stats { get; } = stats;
    public string UserToken { get; } = token;

    public UserStatus ActivityStatus { get; set; }
    public UserState State { get; set; }
}

public sealed record BattleStats(ulong Strength, ulong Defense, ulong Speed, ulong Dexterity)
{
    public ulong Total { get; } = Strength + Defense + Speed + Dexterity;
};

public enum UserStatus
{
    Offline,
    Online,
    Idle
}

public enum UserState
{
    Fallen,
    Okay,
    Hospitialized,
    Traveling,
    Abroad,
    Jail
}
