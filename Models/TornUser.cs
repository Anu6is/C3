namespace C3.Models;

public sealed record TornUser(int Level, int Player_Id, string Name, TornUserState Status, TornUserActivity Last_Action, TornUserFaction Faction) { }

public sealed record TornUserState(string Description, string State, int Until)
{
    public static implicit operator UserState(TornUserState state) => Enum.Parse<UserState>(state.State);
}

public sealed record TornUserActivity(string Status, ulong Timestamp, string Relative)
{
    public static implicit operator UserStatus(TornUserActivity status) => Enum.Parse<UserStatus>(status.Status);
}