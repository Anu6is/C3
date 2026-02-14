namespace C3.Domain.Models;

public sealed record TornUser(int Level,
                              int Player_Id,
                              string Name,
                              ulong Strength,
                              ulong Defense,
                              ulong Speed,
                              ulong Dexterity,
                              int Strength_Modifier,
                              int Defense_Modifier,
                              int Speed_Modifier,
                              int Dexterity_Modifier,
                              TornUserState Status,
                              TornUserActivity Last_Action,
                              TornUserFaction Faction) { }

public sealed record TornUserState(string Description, string State, ulong Until)
{
    public static implicit operator UserState(TornUserState state) => Enum.Parse<UserState>(state.State);
}

public sealed record TornUserActivity(string Status, ulong Timestamp, string Relative)
{
    public static implicit operator UserStatus(TornUserActivity status) => Enum.Parse<UserStatus>(status.Status);
}