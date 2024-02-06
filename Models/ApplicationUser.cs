namespace C3.Models;

public sealed class ApplicationUser(int userId, string userName, TornUserFaction userFaction, string token)
{
    public int UserId { get; } = userId;
    public string Username { get; } = userName;
    public TornUserFaction Faction { get; } = userFaction;
    public string UserToken { get; } = token;

    public UserStatus ActivityStatus { get; set; }
    public UserState State { get; set; }
}

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
