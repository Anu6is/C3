namespace C3.Models;

public sealed class User(int userId, string userName, TornFaction userFaction, string token)
{
    public int UserId { get; } = userId;
    public string UserName { get; } = userName;
    public TornFaction Faction { get; } = userFaction;
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
}
